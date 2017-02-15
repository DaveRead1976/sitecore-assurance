using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using RestSharp;
using Revida.Sitecore.Assurance.Configuration;
using Revida.Sitecore.Services.Client;

namespace Revida.Sitecore.Assurance.Tests
{
    [TestFixture]
    public class SitecoreItemServiceClientTests
    {
        [Test]
        public void Service_client_handles_node_with_no_children()
        {
            // Arrange
            var configurationParameters = new ConfigurationParameters
            {
                BaseUrl = "http://baseurl.com",
                RootNodeId = Guid.NewGuid(),
                SiteCoreClient = SitecoreClientVersion.SiteCoreServicesClient
            };
            var restClient = new Mock<IRestClient>();

            var response = new RestResponse<List<SitecoreItem>> { Data = new List<SitecoreItem>() };

            restClient.Setup(x => x.Execute<List<SitecoreItem>>(It.IsAny<IRestRequest>())).Returns(response);

            var serviceClient = new SitecoreItemServiceClient(restClient.Object, configurationParameters);

            // Act
            var urlList = serviceClient.GetSitecoreCmsTreeUrls();

            // Assert
            Assert.IsNotNull(urlList);
            Assert.IsEmpty(urlList);
        }

        [Test]
        public void Service_client_handles_single_child_node()
        {
            // Arrange
            var configurationParameters = new ConfigurationParameters
            {
                BaseUrl = "http://baseurl.com",
                RootNodeId = Guid.NewGuid(),
                SiteCoreClient = SitecoreClientVersion.SiteCoreServicesClient
            };
            var restClient = new Mock<IRestClient>();

            var itemList = new List<SitecoreItem>
            {
                new SitecoreItem
                {
                    DisplayName = "display name",
                    HasChildren = false,
                    ItemID = Guid.NewGuid(),
                    ItemPath = "/path/to/item",
                    ItemUrl = "http://baseurl.com/item/url"
                }
            };

            var response = new RestResponse<List<SitecoreItem>> { Data = itemList };

            restClient.Setup(x => x.Execute<List<SitecoreItem>>(It.IsAny<IRestRequest>())).Returns(response);

            var serviceClient = new SitecoreItemServiceClient(restClient.Object, configurationParameters);

            // Act
            var urlList = serviceClient.GetSitecoreCmsTreeUrls();

            // Assert
            Assert.IsNotNull(urlList);
            Assert.AreEqual(1, urlList.Count);
        }

        [Test]
        public void Service_client_handles_multiple_child_nodes_with_no_children()
        {
            // Arrange
            var configurationParameters = new ConfigurationParameters
            {
                BaseUrl = "http://baseurl.com",
                RootNodeId = Guid.NewGuid(),
                SiteCoreClient = SitecoreClientVersion.SiteCoreServicesClient
            };
            var restClient = new Mock<IRestClient>();

            var itemList = new List<SitecoreItem>
            {
                new SitecoreItem
                {
                    DisplayName = "display name",
                    HasChildren = false,
                    ItemID = Guid.NewGuid(),
                    ItemPath = "/path/to/item",
                    ItemUrl = "/item/url"
                },
                new SitecoreItem
                {
                    DisplayName = "display name 2",
                    HasChildren = false,
                    ItemID = Guid.NewGuid(),
                    ItemPath = "/path/to/item2",
                    ItemUrl = "/item/url2"
                }
            };

            var response = new RestResponse<List<SitecoreItem>> { Data = itemList };

            restClient.Setup(x => x.Execute<List<SitecoreItem>>(It.IsAny<IRestRequest>())).Returns(response);

            var serviceClient = new SitecoreItemServiceClient(restClient.Object, configurationParameters);

            // Act
            var urlList = serviceClient.GetSitecoreCmsTreeUrls();

            // Assert
            Assert.IsNotNull(urlList);
            Assert.AreEqual(2, urlList.Count);
        }


        [Test]
        public void Service_client_handles_multiple_child_nodes_with_children()
        {
            // Arrange
            var configurationParameters = new ConfigurationParameters
            {
                BaseUrl = "http://baseurl.com",
                RootNodeId = Guid.NewGuid(),
                SiteCoreClient = SitecoreClientVersion.SiteCoreServicesClient
            };
            var restClient = new Mock<IRestClient>();

            var topLevelItemList = new List<SitecoreItem>
            {
                new SitecoreItem
                {
                    DisplayName = "display name",
                    HasChildren = true,
                    ItemID = Guid.NewGuid(),
                    ItemPath = "/path/to/item",
                    ItemUrl = "/item/url"
                },
                new SitecoreItem
                {
                    DisplayName = "display name 2",
                    HasChildren = true,
                    ItemID = Guid.NewGuid(),
                    ItemPath = "/path/to/item2",
                    ItemUrl = "/item/url2"
                }
            };

            var topLevelResponse = new RestResponse<List<SitecoreItem>> { Data = topLevelItemList };

            var firstChildItemList = new List<SitecoreItem>
            {
                new SitecoreItem
                {
                    DisplayName = "display name child 1",
                    HasChildren = false,
                    ItemID = Guid.NewGuid(),
                    ItemPath = "/path/to/item/child1",
                    ItemUrl = "/item/url/child1"
                },
                new SitecoreItem
                {
                    DisplayName = "display name child 2",
                    HasChildren = false,
                    ItemID = Guid.NewGuid(),
                    ItemPath = "/path/to/item/child2",
                    ItemUrl = "/item/url/child2"
                }
            };

            var firstChildResponse = new RestResponse<List<SitecoreItem>> { Data = firstChildItemList };

            var secondChildItemList = new List<SitecoreItem>
            {
                new SitecoreItem
                {
                    DisplayName = "display name child 3",
                    HasChildren = false,
                    ItemID = Guid.NewGuid(),
                    ItemPath = "/path/to/item/child1",
                    ItemUrl = "/item/url2/child3"
                },
                new SitecoreItem
                {
                    DisplayName = "display name child 4",
                    HasChildren = false,
                    ItemID = Guid.NewGuid(),
                    ItemPath = "/path/to/item/child2",
                    ItemUrl = "/item/url2/child4"
                }
            };

            var secondChildResponse = new RestResponse<List<SitecoreItem>> { Data = secondChildItemList };

            restClient.Setup(x => x.Execute<List<SitecoreItem>>(It.IsAny<IRestRequest>()))
                                        .ReturnsInOrder(topLevelResponse, firstChildResponse, secondChildResponse);

            var serviceClient = new SitecoreItemServiceClient(restClient.Object, configurationParameters);

            // Act
            var urlList = serviceClient.GetSitecoreCmsTreeUrls().ToArray();

            // Assert
            Assert.IsNotNull(urlList);
            Assert.AreEqual(6, urlList.Length);
            Assert.AreEqual("/item/url", urlList[0].ItemUrl);
            Assert.AreEqual("/item/url/child1", urlList[1].ItemUrl);
            Assert.AreEqual("/item/url/child2", urlList[2].ItemUrl);
            Assert.AreEqual("/item/url2", urlList[3].ItemUrl);
            Assert.AreEqual("/item/url2/child3", urlList[4].ItemUrl);
            Assert.AreEqual("/item/url2/child4", urlList[5].ItemUrl);
        }

        [Test]
        public void Service_client_handles_multiple_levels_of_children()
        {
            // Arrange
            var configurationParameters = new ConfigurationParameters
            {
                BaseUrl = "http://baseurl.com",
                RootNodeId = Guid.NewGuid(),
                SiteCoreClient = SitecoreClientVersion.SiteCoreServicesClient
            };
            var restClient = new Mock<IRestClient>();

            var topLevelItemList = new List<SitecoreItem>
            {
                new SitecoreItem
                {
                    DisplayName = "display name",
                    HasChildren = true,
                    ItemID = Guid.NewGuid(),
                    ItemPath = "/path/to/item",
                    ItemUrl = "/item/url"
                },
                new SitecoreItem
                {
                    DisplayName = "display name 2",
                    HasChildren = true,
                    ItemID = Guid.NewGuid(),
                    ItemPath = "/path/to/item2",
                    ItemUrl = "/item/url2"
                }
            };

            var topLevelResponse = new RestResponse<List<SitecoreItem>> { Data = topLevelItemList };

            var firstChildItemList = new List<SitecoreItem>
            {
                new SitecoreItem
                {
                    DisplayName = "display name child 1",
                    HasChildren = false,
                    ItemID = Guid.NewGuid(),
                    ItemPath = "/path/to/item/child1",
                    ItemUrl = "/item/url/child1"
                },
                new SitecoreItem
                {
                    DisplayName = "display name child 2",
                    HasChildren = true,
                    ItemID = Guid.NewGuid(),
                    ItemPath = "/path/to/item/child2",
                    ItemUrl = "/item/url/child2"
                }
            };

            var firstChildResponse = new RestResponse<List<SitecoreItem>> { Data = firstChildItemList };

            var child2ChildrenItemList = new List<SitecoreItem>
            {
                new SitecoreItem
                {
                    DisplayName = "display name sub child 1",
                    HasChildren = false,
                    ItemID = Guid.NewGuid(),
                    ItemPath = "/path/to/item/child2/item1",
                    ItemUrl = "/item/url/child2/subchild1"
                },
                new SitecoreItem
                {
                    DisplayName = "display name sub child 2",
                    HasChildren = false,
                    ItemID = Guid.NewGuid(),
                    ItemPath = "/path/to/item/child2/item2",
                    ItemUrl = "/item/url/child2/subchild2"
                }
            };

            var child2ChildrenResponse = new RestResponse<List<SitecoreItem>> { Data = child2ChildrenItemList };

            var secondChildItemList = new List<SitecoreItem>
            {
                new SitecoreItem
                {
                    DisplayName = "display name child 3",
                    HasChildren = true,
                    ItemID = Guid.NewGuid(),
                    ItemPath = "/path/to/item/child1",
                    ItemUrl = "/item/url2/child3"
                },
                new SitecoreItem
                {
                    DisplayName = "display name child 4",
                    HasChildren = false,
                    ItemID = Guid.NewGuid(),
                    ItemPath = "/path/to/item/child2",
                    ItemUrl = "/item/url2/child4"
                }
            };

            var secondChildResponse = new RestResponse<List<SitecoreItem>> { Data = secondChildItemList };
            
            var child3ChildrenItemList = new List<SitecoreItem>
            {
                new SitecoreItem
                {
                    DisplayName = "display name sub child 1",
                    HasChildren = false,
                    ItemID = Guid.NewGuid(),
                    ItemPath = "/path/to/item/child3/item1",
                    ItemUrl = "/item/url/child3/subchild1"
                },
                new SitecoreItem
                {
                    DisplayName = "display name sub child 2",
                    HasChildren = false,
                    ItemID = Guid.NewGuid(),
                    ItemPath = "/path/to/item/child3/item2",
                    ItemUrl = "/item/url/child3/subchild2"
                }
            };

            var child3ChildrenResponse = new RestResponse<List<SitecoreItem>> { Data = child3ChildrenItemList };

            restClient.Setup(x => x.Execute<List<SitecoreItem>>(It.IsAny<IRestRequest>()))
            .ReturnsInOrder(topLevelResponse, firstChildResponse, child2ChildrenResponse, secondChildResponse, child3ChildrenResponse);

            var serviceClient = new SitecoreItemServiceClient(restClient.Object, configurationParameters);

            // Act
            var urlList = serviceClient.GetSitecoreCmsTreeUrls().ToArray();

            // Assert
            Assert.IsNotNull(urlList);
            Assert.AreEqual(10, urlList.Length);
            Assert.AreEqual("/item/url", urlList[0].ItemUrl);
            Assert.AreEqual("/item/url/child1", urlList[1].ItemUrl);
            Assert.AreEqual("/item/url/child2", urlList[2].ItemUrl);
            Assert.AreEqual("/item/url/child2/subchild1", urlList[3].ItemUrl);
            Assert.AreEqual("/item/url/child2/subchild2", urlList[4].ItemUrl);                        
            Assert.AreEqual("/item/url2", urlList[5].ItemUrl);
            Assert.AreEqual("/item/url2/child3", urlList[6].ItemUrl);
            Assert.AreEqual("/item/url/child3/subchild1", urlList[7].ItemUrl);
            Assert.AreEqual("/item/url/child3/subchild2", urlList[8].ItemUrl);
            Assert.AreEqual("/item/url2/child4", urlList[9].ItemUrl);
        }
    }
}
