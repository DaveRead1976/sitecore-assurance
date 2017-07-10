using System;
using System.Collections.Generic;
using RestSharp;
using Revida.Sitecore.Assurance.Configuration;
using Revida.Sitecore.Assurance.Model;

namespace Revida.Sitecore.Services.Client
{
    using System.Net;
    
    public class SitecoreItemServiceClient : ISitecoreServiceClient
    {
        private const string RootItemUrlFormatString = "http://{0}/sitecore/api/ssc/item/{1}";

        private const string ItemChildrenUrlFormatString = "http://{0}/sitecore/api/ssc/item/{1}/children";

        private ConfigurationParameters ConfigurationParameters { get; }

        private IRestClient ServiceClient { get; }

        public SitecoreItemServiceClient(IRestClient serviceClient, ConfigurationParameters configurationParameters)
        {
            ServiceClient = serviceClient;
            ConfigurationParameters = configurationParameters;
        }

        public List<SitecoreItem> GetSitecoreCmsTreeUrls()
        {
            Uri baseUri = new Uri(ConfigurationParameters.BaseUrl);
            List<SitecoreItem> sitecoreUrls = new List<SitecoreItem>();

            Guid rootNodeId = ConfigurationParameters.RootNodeId;
            
            var response = ExecuteRestRequest<SitecoreItem>(baseUri, rootNodeId, RootItemUrlFormatString);
            if (response.StatusCode == HttpStatusCode.OK && response.Data != null)
            {
                sitecoreUrls.Add(response.Data);
                sitecoreUrls = ParseUrlTree(baseUri, rootNodeId, sitecoreUrls);
            }            

            return sitecoreUrls;            
        }

        private List<SitecoreItem> ParseUrlTree(Uri baseUri, Guid rootNodeId, List<SitecoreItem> sitecoreUrls)
        {
            var response = ExecuteRestRequest<List<SitecoreItem>>(baseUri, rootNodeId, ItemChildrenUrlFormatString);

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

        private IRestResponse<T> ExecuteRestRequest<T>(Uri baseUri, Guid rootNodeId, string formatString) where T : new()
        {
            Uri serviceEndpointUrl = new Uri(string.Format(formatString, baseUri.Host, rootNodeId));

            ServiceClient.BaseUrl = baseUri;
            IRestRequest request = new RestRequest(serviceEndpointUrl.PathAndQuery);
            request.Method = Method.GET;

            IRestResponse<T> response = ServiceClient.Execute<T>(request);

            if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                throw new ServiceClientAuthorizationException("Access denied when connecting to Sitecore Service Client");
            }

            return response;
        }
    }
}
 