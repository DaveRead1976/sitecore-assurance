using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using NUnit.Framework;
using Revida.Sitecore.Assurance.Configuration;

namespace Revida.Sitecore.Assurance.Tests
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class CommandLineParserTests
    {
        [Test]
        public void Arguments_are_empty()
        {
            // Arrange
            var args = new string[0];
            
            // Act / Assert
            Assert.Throws<InvalidConfigurationException>(
                () => ConfigurationParameterParser.ParseCommandLineArgs(args),
                "No command line arguments supplied");
        }

        [Test]
        public void Arguments_are_null()
        {
            // Act / Assert
            Assert.Throws<InvalidConfigurationException>(
                () => ConfigurationParameterParser.ParseCommandLineArgs(null),
                "No command line arguments supplied");
        }
        
        [Test]
        public void Arguments_contain_root_node_and_base_url()
        {
            // Arrange
            var args = new string[4];
            var rootNodeGuid = Guid.NewGuid();
            args[0] = "-r";
            args[1] = rootNodeGuid.ToString();
            args[2] = "-b";
            args[3] = "http://www.baseurl.com";
            
            // Act
            var configurationParameters = ConfigurationParameterParser.ParseCommandLineArgs(args);

            // Assert
            Assert.AreEqual(rootNodeGuid, configurationParameters.RootNodeId);            
            Assert.AreEqual("http://www.baseurl.com", configurationParameters.BaseUrl);
        }

        [Test]
        public void Arguments_contain_short_version_root_node_but_no_base_url()
        {
            // Arrange
            var args = new string[4];
            args[0] = "-r";
            var rootNodeGuid = Guid.NewGuid();
            args[1] = rootNodeGuid.ToString();            

            // Act / Assert
            Assert.Throws<InvalidConfigurationException>(
                () => ConfigurationParameterParser.ParseCommandLineArgs(args),
                "Base url is required");
        }
        
        [Test]
        public void Arguments_contain_long_version_root_node_but_no_base_url()
        {
            // Arrange
            var args = new string[2];
            var rootNodeGuid = Guid.NewGuid();
            args[0] = "--root";
            args[1] = rootNodeGuid.ToString();

            // Act / Assert
            Assert.Throws<InvalidConfigurationException>(
                () => ConfigurationParameterParser.ParseCommandLineArgs(args),
                "Base url is required");
        }

        [Test]
        public void Arguments_contain_invalid_url()
        {
            // Arrange
            var args = new string[6];
            var rootNodeGuid = Guid.NewGuid();
            args[0] = "--root";
            args[1] = rootNodeGuid.ToString();            
            args[2] = "--baseurl";
            args[3] = "^7";

            // Act / Assert
            Assert.Throws<InvalidConfigurationException>(
                () => ConfigurationParameterParser.ParseCommandLineArgs(args),
                "Base url is invalid");
        }

        [Test]
        public void Arguments_contain_invalid_root_node_id()
        {
            // Arrange
            var args = new string[6];
            args[0] = "--root";
            args[1] = "abv5";            
            args[2] = "--baseurl";
            args[3] = "http://www.baseurl.com";

            // Act / Assert
            Assert.Throws<InvalidConfigurationException>(
                () => ConfigurationParameterParser.ParseCommandLineArgs(args),
                "Root node id is invalid");
        }

        [Test]
        public void Arguments_contain_long_versions_of_mandatory_values_and_list_urls_switch()
        {
            // Arrange
            var args = new string[5];
            var rootNodeGuid = Guid.NewGuid();
            args[0] = "--root";
            args[1] = rootNodeGuid.ToString();
            args[2] = "--baseurl";
            args[3] = "http://www.baseurl.com";
            args[4] = "--list";

            // Act
            var configurationParameters = ConfigurationParameterParser.ParseCommandLineArgs(args);

            // Assert
            Assert.AreEqual(rootNodeGuid, configurationParameters.RootNodeId);
            Assert.AreEqual("http://www.baseurl.com", configurationParameters.BaseUrl);
            Assert.IsTrue(configurationParameters.ListUrls);
            Assert.IsFalse(configurationParameters.RunHttpChecker);
            Assert.IsFalse(configurationParameters.RunWebDriverChecker);
        }


        [Test]
        public void Arguments_contain_short_versions_of_mandatory_values_and_list_urls_switch()
        {
            // Arrange
            var args = new string[5];
            var rootNodeGuid = Guid.NewGuid();
            args[0] = "-r";
            args[1] = rootNodeGuid.ToString();
            args[2] = "-b";
            args[3] = "http://www.baseurl.com";
            args[4] = "-l";

            // Act
            var configurationParameters = ConfigurationParameterParser.ParseCommandLineArgs(args);

            // Assert
            Assert.AreEqual(rootNodeGuid, configurationParameters.RootNodeId);
            Assert.AreEqual("http://www.baseurl.com", configurationParameters.BaseUrl);
            Assert.IsTrue(configurationParameters.ListUrls);
            Assert.IsFalse(configurationParameters.RunHttpChecker);
            Assert.IsFalse(configurationParameters.RunWebDriverChecker);

        }

        [Test]
        public void Arguments_contain_long_versions_of_mandatory_values_and_http_checker_switch()
        {
            // Arrange
            var args = new string[5];
            var rootNodeGuid = Guid.NewGuid();
            args[0] = "--root";
            args[1] = rootNodeGuid.ToString();
            args[2] = "--baseurl";
            args[3] = "http://www.baseurl.com";
            args[4] = "--http";

            // Act
            var configurationParameters = ConfigurationParameterParser.ParseCommandLineArgs(args);

            // Assert
            Assert.AreEqual(rootNodeGuid, configurationParameters.RootNodeId);
            Assert.AreEqual("http://www.baseurl.com", configurationParameters.BaseUrl);
            Assert.IsTrue(configurationParameters.RunHttpChecker);
            Assert.IsFalse(configurationParameters.ListUrls);
            Assert.IsFalse(configurationParameters.RunWebDriverChecker);
        }

        [Test]
        public void Arguments_contain_short_versions_of_mandatory_values_and_http_checker_switch()
        {
            // Arrange
            var args = new string[5];
            var rootNodeGuid = Guid.NewGuid();
            args[0] = "-r";
            args[1] = rootNodeGuid.ToString();
            args[2] = "-b";
            args[3] = "http://www.baseurl.com";
            args[4] = "-h";

            // Act
            var configurationParameters = ConfigurationParameterParser.ParseCommandLineArgs(args);

            // Assert
            Assert.AreEqual(rootNodeGuid, configurationParameters.RootNodeId);
            Assert.AreEqual("http://www.baseurl.com", configurationParameters.BaseUrl);
            Assert.IsTrue(configurationParameters.RunHttpChecker);
            Assert.IsFalse(configurationParameters.ListUrls);
            Assert.IsFalse(configurationParameters.RunWebDriverChecker);
        }
        
        [Test]
        public void Arguments_contain_long_versions_of_mandatory_values_and_selenium_checker_switch()
        {
            // Arrange
            var args = new string[5];
            var rootNodeGuid = Guid.NewGuid();
            args[0] = "--root";
            args[1] = rootNodeGuid.ToString();
            args[2] = "--baseurl";
            args[3] = "http://www.baseurl.com";
            args[4] = "--selenium";

            // Act
            var configurationParameters = ConfigurationParameterParser.ParseCommandLineArgs(args);

            // Assert
            Assert.AreEqual(rootNodeGuid, configurationParameters.RootNodeId);
            Assert.AreEqual("http://www.baseurl.com", configurationParameters.BaseUrl);
            Assert.IsTrue(configurationParameters.RunWebDriverChecker);
            Assert.IsFalse(configurationParameters.ListUrls);
            Assert.IsFalse(configurationParameters.RunHttpChecker);
        }

        [Test]
        public void Arguments_contain_short_versions_of_mandatory_values_and_selenium_checker_switch()
        {
            // Arrange
            var args = new string[5];
            var rootNodeGuid = Guid.NewGuid();
            args[0] = "-r";
            args[1] = rootNodeGuid.ToString();
            args[2] = "-b";
            args[3] = "http://www.baseurl.com";
            args[4] = "-s";

            // Act
            var configurationParameters = ConfigurationParameterParser.ParseCommandLineArgs(args);

            // Assert
            Assert.AreEqual(rootNodeGuid, configurationParameters.RootNodeId);
            Assert.AreEqual("http://www.baseurl.com", configurationParameters.BaseUrl);
            Assert.IsTrue(configurationParameters.RunWebDriverChecker);
            Assert.IsFalse(configurationParameters.ListUrls);
            Assert.IsFalse(configurationParameters.RunHttpChecker);
        }

        [Test]
        public void Arguments_contain_long_versions_of_mandatory_values_and_service_client_credentials()
        {
            // Arrange
            var args = new string[10];
            var rootNodeGuid = Guid.NewGuid();
            args[0] = "--root";
            args[1] = rootNodeGuid.ToString();
            args[2] = "--baseurl";
            args[3] = "http://www.baseurl.com";
            args[4] = "--username";
            args[5] = "admin";
            args[6] = "--password";
            args[7] = "pass";
            args[8] = "--domain";
            args[9] = "sitecore";

            // Act
            var configurationParameters = ConfigurationParameterParser.ParseCommandLineArgs(args);

            // Assert
            Assert.AreEqual(rootNodeGuid, configurationParameters.RootNodeId);
            Assert.AreEqual("http://www.baseurl.com", configurationParameters.BaseUrl);
            Assert.IsFalse(configurationParameters.RunWebDriverChecker);
            Assert.IsFalse(configurationParameters.ListUrls);
            Assert.IsFalse(configurationParameters.RunHttpChecker);
            Assert.AreEqual("admin", configurationParameters.UserName);
            Assert.AreEqual("pass", configurationParameters.Password);
            Assert.AreEqual("sitecore", configurationParameters.Domain);
        }

        [Test]
        public void Arguments_contain_short_versions_of_mandatory_values_and_service_client_credentials()
        {
            // Arrange
            var args = new string[10];
            var rootNodeGuid = Guid.NewGuid();
            args[0] = "-r";
            args[1] = rootNodeGuid.ToString();
            args[2] = "-b";
            args[3] = "http://www.baseurl.com";
            args[4] = "-u";
            args[5] = "admin";
            args[6] = "-p";
            args[7] = "pass";
            args[8] = "-d";
            args[9] = "sitecore";

            // Act
            var configurationParameters = ConfigurationParameterParser.ParseCommandLineArgs(args);

            // Assert
            Assert.AreEqual(rootNodeGuid, configurationParameters.RootNodeId);
            Assert.AreEqual("http://www.baseurl.com", configurationParameters.BaseUrl);
            Assert.IsFalse(configurationParameters.RunWebDriverChecker);
            Assert.IsFalse(configurationParameters.ListUrls);
            Assert.IsFalse(configurationParameters.RunHttpChecker);
            Assert.AreEqual("admin", configurationParameters.UserName);
            Assert.AreEqual("pass", configurationParameters.Password);
            Assert.AreEqual("sitecore", configurationParameters.Domain);
        }

        [Test]
        public void Arguments_contain_invalid_input_file_name_short_version()
        {
            // Arrange
            var args = new string[6];
            var rootNodeGuid = Guid.NewGuid();
            args[0] = "-r";
            args[1] = rootNodeGuid.ToString();
            args[2] = "-b";
            args[3] = "http://www.baseurl.com";            
            args[4] = "-i";
            args[5] = "this-file-cannot-be-loaded.csv";

            // Act / Assert            
            Assert.Throws<InvalidConfigurationException>(() => ConfigurationParameterParser.ParseCommandLineArgs(args),
                "Invalid input filename supplied");            
        }

        [Test]
        public void Arguments_contain_invalid_input_file_absolute_path_long_version()
        {
            // Arrange
            var args = new string[6];
            var rootNodeGuid = Guid.NewGuid();
            args[0] = "-r";
            args[1] = rootNodeGuid.ToString();
            args[2] = "-b";
            args[3] = "http://www.baseurl.com";
            args[4] = "-i";
            args[5] = "c:\\directory\\this-file-cannot-be-loaded.csv";

            // Act / Assert            
            Assert.Throws<InvalidConfigurationException>(() => ConfigurationParameterParser.ParseCommandLineArgs(args),
                "Invalid input filename supplied");
        }

        [Test, Ignore("Test disabled by default as writes to the file system")]
        public void Arguments_contain_valid_input_file_name()
        {
            // Arrange
            string fileName = $"{Guid.NewGuid()}.csv";

            var testFile = File.Create(fileName);
            testFile.Flush();
            testFile.Close();

            var args = new string[6];
            var rootNodeGuid = Guid.NewGuid();
            args[0] = "-r";
            args[1] = rootNodeGuid.ToString();
            args[2] = "-b";
            args[3] = "http://www.baseurl.com";
            args[4] = "-i";
            args[5] = testFile.Name;

            // Act
            var configurationParameters = ConfigurationParameterParser.ParseCommandLineArgs(args);

            // Assert
            Assert.AreEqual(rootNodeGuid, configurationParameters.RootNodeId);
            Assert.AreEqual("http://www.baseurl.com", configurationParameters.BaseUrl);
            Assert.IsFalse(configurationParameters.RunWebDriverChecker);
            Assert.IsFalse(configurationParameters.ListUrls);
            Assert.IsFalse(configurationParameters.RunHttpChecker);
            Assert.AreEqual(testFile.Name, configurationParameters.InputFileName);
            
            // Clean up
            File.Delete(fileName);
        }
    }
}
