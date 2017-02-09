using System;

namespace Revida.Sitecore.Assurance.Configuration
{
    public class ConfigurationParameters
    {
        public SitecoreClientVersion SiteCoreClient { get; set; }

        public Guid RootNodeId { get; set; }

        public string BaseUrl { get; set; }
    }

    public enum SitecoreClientVersion
    {
        ItemWebApi,
        SiteCoreServicesClient
    }
}
