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

        public string Domain { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string InputFileName { get; set; }

        public bool HasCredentials => (!string.IsNullOrWhiteSpace(Domain)) && (!string.IsNullOrWhiteSpace(UserName)) &&
                                      (!string.IsNullOrWhiteSpace(Password));
    }    
}
