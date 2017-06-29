using System;
using NUnit.Framework;
using Revida.Sitecore.Assurance.Configuration;

namespace Revida.Sitecore.Assurance.Tests
{
    [TestFixture]
    public class CommandLineParserTests
    {
        [Test]
        public void Arguments_are_empty()
        {
            // Arrange
            var args = new string[0];
            
            // Act / Assert
            Assert.Throws<InvalidCommandLineArgumentsException>(
                () => ConfigurationParameterParser.ParseCommandLineArgs(args),
                "No command line arguments supplied");
        }

        [Test]
        public void Arguments_are_null()
        {
            // Arrange
            string[] args = null;
            
            // Act / Assert
            Assert.Throws<InvalidCommandLineArgumentsException>(
                () => ConfigurationParameterParser.ParseCommandLineArgs(args),
                "No command line arguments supplied");
        }
        
        [Test]
        public void Arguments_contain_root_node_and_base_url()
        {
            // Arrange
            var args = new string[4];
            Guid rootNodeGuid = Guid.NewGuid();
            args[0] = "-r";
            args[1] = rootNodeGuid.ToString();
            args[2] = "-u";
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
            Guid rootNodeGuid = Guid.NewGuid();
            args[1] = rootNodeGuid.ToString();            

            // Act / Assert
            Assert.Throws<InvalidCommandLineArgumentsException>(
                () => ConfigurationParameterParser.ParseCommandLineArgs(args),
                "Base url is required");
        }
        
        [Test]
        public void Arguments_contain_long_version_root_node_but_no_base_url()
        {
            // Arrange
            var args = new string[2];
            Guid rootNodeGuid = Guid.NewGuid();
            args[0] = "--root";
            args[1] = rootNodeGuid.ToString();

            // Act / Assert
            Assert.Throws<InvalidCommandLineArgumentsException>(
                () => ConfigurationParameterParser.ParseCommandLineArgs(args),
                "Base url is required");
        }

        [Test]
        public void Arguments_contain_invalid_url()
        {
            // Arrange
            var args = new string[6];
            Guid rootNodeGuid = Guid.NewGuid();
            args[0] = "--root";
            args[1] = rootNodeGuid.ToString();            
            args[2] = "--baseurl";
            args[3] = "^7";

            // Act / Assert
            Assert.Throws<InvalidCommandLineArgumentsException>(
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
            args[2] = "--service";
            args[3] = "6";
            args[4] = "--baseurl";
            args[5] = "http://www.baseurl.com";

            // Act / Assert
            Assert.Throws<InvalidCommandLineArgumentsException>(
                () => ConfigurationParameterParser.ParseCommandLineArgs(args),
                "Root node id is invalid");
        }
    }
}
