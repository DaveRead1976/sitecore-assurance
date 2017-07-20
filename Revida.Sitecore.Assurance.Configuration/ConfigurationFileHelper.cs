using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;

namespace Revida.Sitecore.Assurance.Configuration
{    
    [ExcludeFromCodeCoverage]
    public static class ConfigurationFileHelper
    {
        public static bool LoadConfiguration(string[] args, out ConfigurationParameters config)
        {
            try
            {
                if (ConfigurationFileExists())
                {
                    try
                    {
                        config = ConfigurationParameterParser.LoadConfigurationFile();
                    }
                    catch (InvalidConfigurationException)
                    {
                        // fall back to command line options if cannot load from .config
                        config = ConfigurationParameterParser.ParseCommandLineArgs(args);
                    }
                }
                else
                {
                    config = ConfigurationParameterParser.ParseCommandLineArgs(args);
                }
            }
            catch (InvalidConfigurationException)
            {
                config = null;
                return false;
            }
            return true;
        }
        private static bool ConfigurationFileExists()
        {
            var assembly = Assembly.GetExecutingAssembly();

            return File.Exists(assembly.Location + ".config");
        }

    }
}
