using System;
using System.Collections.Generic;
using RestSharp;
using Revida.Sitecore.Assurance.Configuration;

namespace Revida.Sitecore.Services.Client
{
    public class SitecoreItemServiceClient : ISitecoreServiceClient
    {
        private const string ItemUrlFormatString = "http://{0}/sitecore/api/ssc/item/{1}/children";

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

            return ParseUrlTree(baseUri, rootNodeId, sitecoreUrls); 
        }

        private List<SitecoreItem> ParseUrlTree(Uri baseUri, Guid rootNodeId, List<SitecoreItem> sitecoreUrls)
        {
            Uri serviceEndpointUrl = new Uri(string.Format(ItemUrlFormatString, baseUri.Host, rootNodeId));

            ServiceClient.BaseUrl = baseUri;
            IRestRequest request = new RestRequest(serviceEndpointUrl.PathAndQuery);
            request.Method = Method.GET;
            
            IRestResponse<List<SitecoreItem>> response = ServiceClient.Execute<List<SitecoreItem>>(request);
            
            List<SitecoreItem> items = response.Data;

            foreach (SitecoreItem item in items)
            {
                sitecoreUrls.Add(item);
                if (item.HasChildren)
                {
                    sitecoreUrls = ParseUrlTree(baseUri, item.ItemID, sitecoreUrls);
                }
            }
            return sitecoreUrls;
        }
    }
}
 