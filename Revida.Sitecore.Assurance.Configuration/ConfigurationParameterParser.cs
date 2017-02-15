using System;
using CommandLine;

namespace Revida.Sitecore.Assurance.Configuration
{
    public class ConfigurationParameterParser
    {
        public static ConfigurationParameters ParseCommandLineArgs(string[] commandLineArgs)
        {
            if (commandLineArgs == null || commandLineArgs.Length == 0)
            {
                throw new InvalidCommandLineArgumentsException("No command line arguments supplied");
            }

            var parser = new Parser();
            var options = new CommandLineParameters();
            try
            {
                parser.ParseArguments(commandLineArgs, options);
            }
            catch (ArgumentNullException)
            {
                throw new InvalidCommandLineArgumentsException("No command line arguments supplied");
            }

            return BuildConfigurationParameters(options);
        }

        private static ConfigurationParameters BuildConfigurationParameters(CommandLineParameters options)
        {
            var parameters = new ConfigurationParameters();

            ParseRootNodeParameter(options.Root, parameters);

            ParseBaseUrlParameter(options.BaseUrl, parameters);

            ParseOptionalServiceVersionParameter(options.Service, parameters);

            return parameters;
        }

        private static void ParseRootNodeParameter(string root, ConfigurationParameters parameters)
        {
            if (!String.IsNullOrEmpty(root))
            {
                Guid rootNodeGuid;
                var isValid = Guid.TryParse(root, out rootNodeGuid);
                if (isValid)
                {
                    parameters.RootNodeId = rootNodeGuid;
                }
                else
                {
                    throw new InvalidCommandLineArgumentsException("Root node id is invalid");
                }
            }
            else
            {
                throw new InvalidCommandLineArgumentsException("Root node id is required");
            }
        }

        private static void ParseBaseUrlParameter(string baseUrl, ConfigurationParameters parameters)
        {
            if (!String.IsNullOrEmpty(baseUrl))
            {
                Uri parseUri; 
                bool isValid = Uri.TryCreate(baseUrl, UriKind.Absolute, out parseUri);
                if (isValid)
                {
                    parameters.BaseUrl = baseUrl;
                    return;
                }
            }
            throw new InvalidCommandLineArgumentsException("Base url is required");        
        }

        private static void ParseOptionalServiceVersionParameter(string service, ConfigurationParameters parameters)
        {
            if (!String.IsNullOrEmpty(service))
            {
                if (service == "6")
                {
                    parameters.SiteCoreClient = SitecoreClientVersion.ItemWebApi;
                }
            }
            else
            {
                parameters.SiteCoreClient = SitecoreClientVersion.SiteCoreServicesClient;
            }
        }

    }
}
