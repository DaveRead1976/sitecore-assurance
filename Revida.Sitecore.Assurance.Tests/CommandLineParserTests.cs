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
            Assert.Throws<InvalidCommandLineArgumentsException>(() => ConfigurationParameterParser.ParseCommandLineArgs(args));
        }

        [Test]
        public void Arguments_are_null()
        {
            // Arrange
            string[] args = null;
            
            // Act / Assert
            Assert.Throws<InvalidCommandLineArgumentsException>(() => ConfigurationParameterParser.ParseCommandLineArgs(args));
        }
        
        [Test]
        public void Arguments_contain_root_node_but_no_sitecore_version()
        {
            // Arrange
            var args = new string[2];
            Guid rootNodeGuid = Guid.NewGuid();
            args[0] = "-r";
            args[1] = rootNodeGuid.ToString();
            
            // Act
            var configurationParameters = ConfigurationParameterParser.ParseCommandLineArgs(args);

            // Assert
            Assert.AreEqual(rootNodeGuid, configurationParameters.RootNodeId);
            Assert.AreEqual(SitecoreClientVersion.SiteCoreServicesClient, configurationParameters.SiteCoreClient);
        }

        [Test]
        public void Arguments_contain_root_node_and_sitecore_version()
        {
            // Arrange
            var args = new string[4];
            Guid rootNodeGuid = Guid.NewGuid();
            args[0] = "-r";
            args[1] = rootNodeGuid.ToString();
            args[2] = "-s";
            args[3] = "6";
            
            // Act
            var configurationParameters = ConfigurationParameterParser.ParseCommandLineArgs(args);

            // Assert
            Assert.AreEqual(rootNodeGuid, configurationParameters.RootNodeId);
            Assert.AreEqual(SitecoreClientVersion.ItemWebApi, configurationParameters.SiteCoreClient);
        }

        [Test]
        public void Arguments_contain_sitecore_version_but_no_root_node()
        {
            // Arrange
            var args = new string[2];            
            args[0] = "-s";
            args[1] = "6";
            
            // Act / Assert
            Assert.Throws<InvalidCommandLineArgumentsException>(() => ConfigurationParameterParser.ParseCommandLineArgs(args));
        }
    }
}
