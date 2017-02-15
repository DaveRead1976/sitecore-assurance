using RestSharp;
using Revida.Sitecore.Assurance.Configuration;

namespace Revida.Sitecore.Services.Client
{
    public class SitecoreClientFactory
    {
        private ConfigurationParameters ConfigurationParameters { get; }

        private IRestClient ServiceClient { get; }

        public SitecoreClientFactory(IRestClient serviceClient, ConfigurationParameters configurationSettings)
        {
            ServiceClient = serviceClient;
            ConfigurationParameters = configurationSettings;
        }

        public ISitecoreServiceClient GetServiceClient()
        {
            switch (ConfigurationParameters.SiteCoreClient)
            {
                case SitecoreClientVersion.ItemWebApi:
                    return new SitecoreWebApiServiceClient(ServiceClient, ConfigurationParameters);

                default:
                    return new SitecoreItemServiceClient(ServiceClient, ConfigurationParameters);
            }

        }
    }
}
