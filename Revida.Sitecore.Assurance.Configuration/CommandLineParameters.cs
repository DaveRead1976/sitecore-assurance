using CommandLine;

namespace Revida.Sitecore.Assurance.Configuration
{
    public class CommandLineParameters
    {
        [Option('r', "root", Required = true)]
        public string Root { get; set; }
        
        [Option('b', "baseurl")]
        public string BaseUrl { get; set; }

        [Option('d', "domain")]
        public string Domain { get; set; }

        [Option('u', "username")]
        public string Username { get; set; }

        [Option('p', "password")]
        public string Password { get; set; }

        [Option('l', "list")]
        public bool ListUrls { get; set; }

        [Option('h', "http")]
        public bool RunHttpChecker { get; set; }

        [Option('s', "selenium")]
        public bool RunWebDriverChecker { get; set; }
    }
}
