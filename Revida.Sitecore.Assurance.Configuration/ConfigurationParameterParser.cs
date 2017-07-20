using System;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Configuration;
using System.IO;
using System.Reflection;
using CommandLine;

namespace Revida.Sitecore.Assurance.Configuration
{

    public class ConfigurationParameterParser
    {
        public static ConfigurationParameters ParseCommandLineArgs(string[] commandLineArgs)
        {
            if (commandLineArgs == null || commandLineArgs.Length == 0)
            {
                throw new InvalidConfigurationException("No command line arguments supplied");
            }

            var parser = new Parser();
            var options = new CommandLineParameters();
            try
            {
                parser.ParseArguments(commandLineArgs, options);
            }
            catch (ArgumentNullException)
            {
                throw new InvalidConfigurationException("Invalid command line arguments supplied");
            }

            return BuildConfigurationParameters(options);
        }

        [ExcludeFromCodeCoverage]
        public static ConfigurationParameters LoadConfigurationFile()
        {
            var configurationSettings = ConfigurationManager.GetSection("SitecoreAssurance") as NameValueCollection;

            if (configurationSettings == null)
            {
                throw new InvalidConfigurationException("No Sitecore Assurance configuration section found in config file");
            }

            try
            {
                var configurationParameters = new ConfigurationParameters
                {
                    BaseUrl = configurationSettings["BaseUrl"],
                    ListUrls = Convert.ToBoolean(configurationSettings["ListUrls"]),
                    RootNodeId = new Guid(configurationSettings["RootNodeId"]),
                    RunHttpChecker = Convert.ToBoolean(configurationSettings["RunHttpChecker"]),
                    RunWebDriverChecker = Convert.ToBoolean(configurationSettings["RunWebDriverChecker"]),
                    Domain = configurationSettings["Domain"],
                    UserName = configurationSettings["UserName"],
                    Password = configurationSettings["Password"],
                    InputFileName = configurationSettings["InputFilename"]
                };
                return configurationParameters;
            }
            catch (Exception)
            {
                throw new InvalidConfigurationException("Invalid value(s) in Sitecore Assurance configuration section");
            }
        }

        private static ConfigurationParameters BuildConfigurationParameters(CommandLineParameters options)
        {
            var parameters = new ConfigurationParameters();

            ParseRootNodeParameter(options.Root, parameters);

            ParseBaseUrlParameter(options.BaseUrl, parameters);

            ParseInputFilenameParameter(options.InputFileName, parameters);

            parameters.ListUrls = options.ListUrls;
            parameters.RunHttpChecker = options.RunHttpChecker;
            parameters.RunWebDriverChecker = options.RunWebDriverChecker;
            parameters.Domain = options.Domain;
            parameters.UserName = options.Username;
            parameters.Password = options.Password;
            
            return parameters;
        }
        
        private static void ParseRootNodeParameter(string root, ConfigurationParameters parameters)
        {
            if (!string.IsNullOrEmpty(root))
            {
                Guid rootNodeGuid;
                var isValid = Guid.TryParse(root, out rootNodeGuid);
                if (isValid)
                {
                    parameters.RootNodeId = rootNodeGuid;
                }
                else
                {
                    throw new InvalidConfigurationException("Root node id is invalid");
                }
            }
            else
            {
                throw new InvalidConfigurationException("Root node id is required");
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
            throw new InvalidConfigurationException("Base url is required");        
        }
        
        private static void ParseInputFilenameParameter(string inputFileName, ConfigurationParameters parameters)
        {
            if (string.IsNullOrEmpty(inputFileName))
            {
                return;
            }

            // test if fully qualified path 
            if (File.Exists(inputFileName))
            {
                parameters.InputFileName = inputFileName;
                return;
            }

            // test if path relative to executable
            var assembly = Assembly.GetExecutingAssembly();            
            var absoluteFilePath = Path.Combine(assembly.Location, inputFileName);

            if (!File.Exists(absoluteFilePath))
            {
                throw new InvalidConfigurationException("Invalid input filename supplied");
            }

            parameters.InputFileName = absoluteFilePath;            
        }
    }
}
