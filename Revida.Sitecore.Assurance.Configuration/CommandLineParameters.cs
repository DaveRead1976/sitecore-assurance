
using CommandLine;

namespace Revida.Sitecore.Assurance.Configuration
{
    public class CommandLineParameters
    {
        [Option('r', "root", Required = true)]
        public string Root { get; set; }

        [Option('s', "service")]
        public string Service { get; set; }

        [Option('u', "baseurl")]
        public string BaseUrl { get; set; }
    }
}
