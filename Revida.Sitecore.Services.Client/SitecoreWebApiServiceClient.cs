using System;
using System.Collections.Generic;
using RestSharp;
using Revida.Sitecore.Assurance.Configuration;

namespace Revida.Sitecore.Services.Client
{
    public class SitecoreWebApiServiceClient : ISitecoreServiceClient
    {
        private ConfigurationParameters ConfigurationParameters { get; }

        private IRestClient ServiceClient { get; }

        public SitecoreWebApiServiceClient(IRestClient serviceClient, ConfigurationParameters configurationParameters)
        {
            ServiceClient = serviceClient;
            ConfigurationParameters = configurationParameters;
        }
        
        public List<string> GetSitecoreCmsTreeUrls()
        {
            throw new NotImplementedException();
        }
    }
}
 