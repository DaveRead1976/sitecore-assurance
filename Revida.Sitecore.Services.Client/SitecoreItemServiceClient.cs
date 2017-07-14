using System;
using System.Collections.Generic;
using System.Net;
using RestSharp;
using Revida.Sitecore.Assurance.Configuration;
using Revida.Sitecore.Assurance.Model;

namespace Revida.Sitecore.Services.Client
{
    public class SitecoreItemServiceClient : ISitecoreServiceClient
    {
        private const string RootItemUrlFormatString = "http://{0}/sitecore/api/ssc/item/{1}";

        private const string ItemChildrenUrlFormatString = "http://{0}/sitecore/api/ssc/item/{1}/children";

        private const string LoginUrlFormatString = "https://{0}/sitecore/api/ssc/auth/login";

        private const string LogoutUrlFormatString = "https://{0}/sitecore/api/ssc/auth/logout";

        private ConfigurationParameters ConfigurationParameters { get; }

        private IRestClient ServiceClient { get; }

        public SitecoreItemServiceClient(IRestClient serviceClient, ConfigurationParameters configurationParameters)
        {
            ServiceClient = serviceClient;
            ConfigurationParameters = configurationParameters;
            // allow self-signed certificates to be trusted for ssl connection
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
        }

        public List<SitecoreItem> GetSitecoreCmsTreeUrls()
        {
            Uri baseUri = new Uri(ConfigurationParameters.BaseUrl);
            List<SitecoreItem> sitecoreUrls = new List<SitecoreItem>();

            if (ConfigurationParameters.HasCredentials)
            {
                bool loginSucess = Login();
                if (!loginSucess)
                {
                    return sitecoreUrls;
                }
            }
            
            Guid rootNodeId = ConfigurationParameters.RootNodeId;
            
            var response = ExecuteRestRequest<SitecoreItem>(baseUri, RootItemUrlFormatString, Method.GET, rootNodeId.ToString());
            if (response.StatusCode == HttpStatusCode.OK && response.Data != null)
            {
                sitecoreUrls.Add(response.Data);
                sitecoreUrls = ParseUrlTree(baseUri, rootNodeId, sitecoreUrls);
            }

            if (ConfigurationParameters.HasCredentials)
            {
                Logout();
            }

            return sitecoreUrls;            
        }

        public bool Login()
        {
            Uri baseUri = new Uri(ConfigurationParameters.BaseUrl);
            
            var response = ExecuteRestRequest<LoginResult>(baseUri, LoginUrlFormatString, Method.POST, null, BuildCredentials());

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }
            return false;
        }

        public bool Logout()
        {
            Uri baseUri = new Uri(ConfigurationParameters.BaseUrl);

            var response = ExecuteRestRequest<LogoutResult>(baseUri, LogoutUrlFormatString, Method.POST);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }
            return false;
        }

        private List<SitecoreItem> ParseUrlTree(Uri baseUri, Guid rootNodeId, List<SitecoreItem> sitecoreUrls)
        {
            var response = ExecuteRestRequest<List<SitecoreItem>>(baseUri, ItemChildrenUrlFormatString, Method.GET, rootNodeId.ToString());

            if (response.StatusCode == HttpStatusCode.OK && response.Data != null)
            {
                List<SitecoreItem> items = response.Data;

                foreach (SitecoreItem item in items)
                {
                    sitecoreUrls.Add(item);
                    if (item.HasChildren)
                    {
                        sitecoreUrls = ParseUrlTree(baseUri, item.ItemID, sitecoreUrls);
                    }
                }
            }
            return sitecoreUrls;
        }

        private IRestResponse<T> ExecuteRestRequest<T>(Uri baseUri, string formatString, Method requestMethod, string rootNodeId = null, Credentials credentials = null) where T : new()
        {
            Uri serviceEndpointUrl = new Uri(string.Format(formatString, baseUri.Host, rootNodeId));

            ServiceClient.BaseUrl = baseUri;
            IRestRequest request = new RestRequest(serviceEndpointUrl.PathAndQuery);
            request.Method = requestMethod;
            request.RequestFormat = DataFormat.Json;
            if (credentials != null && requestMethod == Method.POST)
            {
                request.AddBody(credentials);
            }

            IRestResponse<T> response = ServiceClient.Execute<T>(request);

            if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                throw new ServiceClientAuthorizationException("Access denied when connecting to Sitecore Service Client");
            }

            return response;
        }

        private Credentials BuildCredentials()
        {
            return new Credentials
            {
                Domain = ConfigurationParameters.Domain,
                Username = ConfigurationParameters.UserName,
                Password = ConfigurationParameters.Password
            };
        }
    }
}
 