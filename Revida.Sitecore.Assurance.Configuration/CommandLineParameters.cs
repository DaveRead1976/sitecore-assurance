using CommandLine;

namespace Revida.Sitecore.Assurance.Configuration
{
    public class CommandLineParameters
    {
        [Option('r', "root", Required = true)]
        public string Root { get; set; }
        
        [Option('u', "baseurl")]
        public string BaseUrl { get; set; }

        [Option('l', "list")]
        public bool ListUrls { get; set; }

        [Option('h', "http")]
        public bool RunHttpChecker { get; set; }

        [Option('s', "selenium")]
        public bool RunWebDriverChecker { get; set; }
    }
}
