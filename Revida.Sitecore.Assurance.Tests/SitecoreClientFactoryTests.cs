using System;
using Moq;
using NUnit.Framework;
using RestSharp;
using Revida.Sitecore.Assurance.Configuration;
using Revida.Sitecore.Services.Client;

namespace Revida.Sitecore.Assurance.Tests
{
    [TestFixture]
    public class SitecoreClientFactoryTests
    {
        [Test]
        public void Factory_returns_Sitecore_6_item_web_api_service_client()
        {
            // Arrange
            var configurationParameters = new ConfigurationParameters
            {
                BaseUrl = "http://baseurl.com",
                RootNodeId = Guid.NewGuid(),
                SiteCoreClient = SitecoreClientVersion.ItemWebApi
            };
            var restClient = new Mock<IRestClient>();

            var factory = new SitecoreClientFactory(restClient.Object, configurationParameters);

            // Act
            var client = factory.GetServiceClient();

            // Assert
            Assert.IsInstanceOf<SitecoreWebApiServiceClient>(client);
        }

        [Test]
        public void Factory_returns_Sitecore_7_and_above_service_client()
        {
            // Arrange
            var configurationParameters = new ConfigurationParameters
            {
                BaseUrl = "http://baseurl.com",
                RootNodeId = Guid.NewGuid(),
                SiteCoreClient = SitecoreClientVersion.SiteCoreServicesClient
            };
            var restClient = new Mock<IRestClient>();

            var factory = new SitecoreClientFactory(restClient.Object, configurationParameters);

            // Act
            var client = factory.GetServiceClient();

            // Assert
            Assert.IsInstanceOf<SitecoreItemServiceClient>(client);
        }
    }
}
