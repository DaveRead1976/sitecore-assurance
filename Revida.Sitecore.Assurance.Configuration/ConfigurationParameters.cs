using System;

namespace Revida.Sitecore.Assurance.Configuration
{
    public class ConfigurationParameters
    {
        public Guid RootNodeId { get; set; }

        public string BaseUrl { get; set; }

        public bool ListUrls { get; set; }

        public bool RunHttpChecker { get; set; }

        public bool RunWebDriverChecker { get; set; }
    }    
}
