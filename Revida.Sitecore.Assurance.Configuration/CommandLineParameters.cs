
using CommandLine;

namespace Revida.Sitecore.Assurance.Configuration
{
    public class CommandLineParameters
    {
        [Option('r', "root")]
        public string Root { get; set; }

        [Option('s', "service")]
        public string Service { get; set; }
    }
}
