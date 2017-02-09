
using Revida.Sitecore.Assurance.Configuration;

namespace Revida.Sitecore.Services.Client
{
    public class SitecoreClientFactory
    {
        private ConfigurationParameters ConfigurationParameters { get; set; }

        public SitecoreClientFactory(ConfigurationParameters configurationSettings)
        {
            ConfigurationParameters = configurationSettings;
        }

        public ISitecoreServiceClient GetServiceClient()
        {
            switch (ConfigurationParameters.SiteCoreClient)
            {
                case SitecoreClientVersion.ItemWebApi:
                    return new SitecoreWebApiServiceClient();

                default:
                    return new SitecoreItemServiceClient();
            }

        }
    }
}
