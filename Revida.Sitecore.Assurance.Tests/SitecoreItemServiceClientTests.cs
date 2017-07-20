using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using Moq;
using NUnit.Framework;
using RestSharp;
using Revida.Sitecore.Assurance.Configuration;
using Revida.Sitecore.Services.Client;

namespace Revida.Sitecore.Assurance.Tests
{
    using Model;

    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class SitecoreItemServiceClientTests
    {
        [Test]
        public void Service_client_handles_node_with_no_children()
        {
            // Arrange
            var configurationParameters = new ConfigurationParameters
            {
                BaseUrl = "http://baseurl.com",
                RootNodeId = Guid.NewGuid()                
            };

            var rootNode = new SitecoreItem
            {
                DisplayName = "display name",
                HasChildren = false,
                ItemID = Guid.NewGuid(),
                ItemPath = "/path/to/item",
                ExternalUrl = "/item/root"
            };

            var restClient = new Mock<IRestClient>();

            var rootResponse = new RestResponse<SitecoreItem> { Data = rootNode, StatusCode = HttpStatusCode.OK };

            var response = new RestResponse<List<SitecoreItem>> { Data = new List<SitecoreItem>(), StatusCode = HttpStatusCode.OK };

            restClient.Setup(x => x.Execute<SitecoreItem>(It.IsAny<IRestRequest>())).Returns(rootResponse);

            restClient.Setup(x => x.Execute<List<SitecoreItem>>(It.IsAny<IRestRequest>())).Returns(response);

            var serviceClient = new SitecoreItemServiceClient(restClient.Object, configurationParameters);

            // Act
            var urlList = serviceClient.GetSitecoreCmsTreeUrls();

            // Assert
            Assert.IsNotNull(urlList);
            Assert.AreEqual(1, urlList.Count);    // include root node but no children
        }

        [Test]
        public void Service_client_handles_null_response_for_root_node()
        {
            // Arrange
            var configurationParameters = new ConfigurationParameters
            {
                BaseUrl = "http://baseurl.com",
                RootNodeId = Guid.NewGuid()
            };
            
            var restClient = new Mock<IRestClient>();

            var rootResponse = new RestResponse<SitecoreItem> { Data = null, StatusCode = HttpStatusCode.OK };
            
            restClient.Setup(x => x.Execute<SitecoreItem>(It.IsAny<IRestRequest>())).Returns(rootResponse);
            
            var serviceClient = new SitecoreItemServiceClient(restClient.Object, configurationParameters);

            // Act
            var urlList = serviceClient.GetSitecoreCmsTreeUrls();

            // Assert
            Assert.IsNotNull(urlList);
            Assert.AreEqual(0, urlList.Count);
        }

        [Test]
        public void Service_client_handles_null_response_for_children()
        {
            // Arrange
            var configurationParameters = new ConfigurationParameters
            {
                BaseUrl = "http://baseurl.com",
                RootNodeId = Guid.NewGuid()
            };

            var rootNode = new SitecoreItem
            {
                DisplayName = "display name",
                HasChildren = false,
                ItemID = Guid.NewGuid(),
                ItemPath = "/path/to/item",
                ExternalUrl = "/item/root"
            };

            var restClient = new Mock<IRestClient>();

            var rootResponse = new RestResponse<SitecoreItem> { Data = rootNode, StatusCode = HttpStatusCode.OK };

            var response = new RestResponse<List<SitecoreItem>> { Data = null, StatusCode = HttpStatusCode.OK };

            restClient.Setup(x => x.Execute<SitecoreItem>(It.IsAny<IRestRequest>())).Returns(rootResponse);

            restClient.Setup(x => x.Execute<List<SitecoreItem>>(It.IsAny<IRestRequest>())).Returns(response);

            var serviceClient = new SitecoreItemServiceClient(restClient.Object, configurationParameters);

            // Act
            var urlList = serviceClient.GetSitecoreCmsTreeUrls();

            // Assert
            Assert.IsNotNull(urlList);
            Assert.AreEqual(1, urlList.Count);
        }

        [Test]
        public void Service_client_handles_single_child_node()
        {
            // Arrange
            var configurationParameters = new ConfigurationParameters
            {
                BaseUrl = "http://baseurl.com",
                RootNodeId = Guid.NewGuid()
            };

            var restClient = new Mock<IRestClient>();

            var rootNode = new SitecoreItem
            {
                DisplayName = "display name",
                HasChildren = false,
                ItemID = Guid.NewGuid(),
                ItemPath = "/path/to/item",
                ExternalUrl = "/item/root"
            };

            var itemList = new List<SitecoreItem>
            {
                new SitecoreItem
                {
                    DisplayName = "display name",
                    HasChildren = false,
                    ItemID = Guid.NewGuid(),
                    ItemPath = "/path/to/item",
                    ExternalUrl = "http://baseurl.com/item/url"
                }
            };

            var rootResponse = new RestResponse<SitecoreItem> { Data = rootNode, StatusCode = HttpStatusCode.OK };

            var response = new RestResponse<List<SitecoreItem>> { Data = itemList, StatusCode = HttpStatusCode.OK };

            restClient.Setup(x => x.Execute<SitecoreItem>(It.IsAny<IRestRequest>())).Returns(rootResponse);

            restClient.Setup(x => x.Execute<List<SitecoreItem>>(It.IsAny<IRestRequest>())).Returns(response);

            var serviceClient = new SitecoreItemServiceClient(restClient.Object, configurationParameters);

            // Act
            var urlList = serviceClient.GetSitecoreCmsTreeUrls();

            // Assert
            Assert.IsNotNull(urlList);
            Assert.AreEqual(2, urlList.Count);
        }

        [Test]
        public void Service_client_handles_multiple_child_nodes_with_no_children()
        {
            // Arrange
            var configurationParameters = new ConfigurationParameters
            {
                BaseUrl = "http://baseurl.com",
                RootNodeId = Guid.NewGuid()
            };
            var restClient = new Mock<IRestClient>();

            var rootNode = new SitecoreItem
            {
                DisplayName = "display name",
                HasChildren = false,
                ItemID = Guid.NewGuid(),
                ItemPath = "/path/to/item",
                ExternalUrl = "/item/root"
            };

            var itemList = new List<SitecoreItem>
            {
                new SitecoreItem
                {
                    DisplayName = "display name",
                    HasChildren = false,
                    ItemID = Guid.NewGuid(),
                    ItemPath = "/path/to/item",
                    ExternalUrl = "/item/url"
                },
                new SitecoreItem
                {
                    DisplayName = "display name 2",
                    HasChildren = false,
                    ItemID = Guid.NewGuid(),
                    ItemPath = "/path/to/item2",
                    ExternalUrl = "/item/url2"
                }
            };

            var rootResponse = new RestResponse<SitecoreItem> { Data = rootNode, StatusCode = HttpStatusCode.OK };

            var response = new RestResponse<List<SitecoreItem>> { Data = itemList, StatusCode = HttpStatusCode.OK };

            restClient.Setup(x => x.Execute<SitecoreItem>(It.IsAny<IRestRequest>())).Returns(rootResponse);

            restClient.Setup(x => x.Execute<List<SitecoreItem>>(It.IsAny<IRestRequest>())).Returns(response);

            var serviceClient = new SitecoreItemServiceClient(restClient.Object, configurationParameters);

            // Act
            var urlList = serviceClient.GetSitecoreCmsTreeUrls();

            // Assert
            Assert.IsNotNull(urlList);
            Assert.AreEqual(3, urlList.Count);
        }


        [Test]
        public void Service_client_handles_multiple_child_nodes_with_children()
        {
            // Arrange
            var configurationParameters = new ConfigurationParameters
            {
                BaseUrl = "http://baseurl.com",
                RootNodeId = Guid.NewGuid()
            };
            var restClient = new Mock<IRestClient>();

            var topLevelItem = new SitecoreItem
            {
                DisplayName = "root node",
                HasChildren = true,
                ItemID = Guid.NewGuid(),
                ItemPath = "/path/to/item",
                ExternalUrl = "/item/home"
            };

            var topLevelResponse = new RestResponse<SitecoreItem> { Data = topLevelItem, StatusCode = HttpStatusCode.OK };

            var topLevelItemList = new List<SitecoreItem>
            {
                new SitecoreItem
                {
                    DisplayName = "display name",
                    HasChildren = true,
                    ItemID = Guid.NewGuid(),
                    ItemPath = "/path/to/item",
                    ExternalUrl = "/item/url"
                },
                new SitecoreItem
                {
                    DisplayName = "display name 2",
                    HasChildren = true,
                    ItemID = Guid.NewGuid(),
                    ItemPath = "/path/to/item2",
                    ExternalUrl = "/item/url2"
                }
            };
            var topLevelChildrenResponse = new RestResponse<List<SitecoreItem>> {  Data = topLevelItemList, StatusCode = HttpStatusCode.OK };

            var firstChildItemList = new List<SitecoreItem>
            {
                new SitecoreItem
                {
                    DisplayName = "display name child 1",
                    HasChildren = false,
                    ItemID = Guid.NewGuid(),
                    ItemPath = "/path/to/item/child1",
                    ExternalUrl = "/item/url/child1"
                },
                new SitecoreItem
                {
                    DisplayName = "display name child 2",
                    HasChildren = false,
                    ItemID = Guid.NewGuid(),
                    ItemPath = "/path/to/item/child2",
                    ExternalUrl = "/item/url/child2"
                }
            };

            var firstSubChildResponse = new RestResponse<List<SitecoreItem>> { Data = firstChildItemList, StatusCode = HttpStatusCode.OK };

            var secondChildItemList = new List<SitecoreItem>
            {
                new SitecoreItem
                {
                    DisplayName = "display name child 3",
                    HasChildren = false,
                    ItemID = Guid.NewGuid(),
                    ItemPath = "/path/to/item/child1",
                    ExternalUrl = "/item/url2/child3"
                },
                new SitecoreItem
                {
                    DisplayName = "display name child 4",
                    HasChildren = false,
                    ItemID = Guid.NewGuid(),
                    ItemPath = "/path/to/item/child2",
                    ExternalUrl = "/item/url2/child4"
                }
            };

            var secondSubChildResponse = new RestResponse<List<SitecoreItem>> { Data = secondChildItemList, StatusCode = HttpStatusCode.OK };

            restClient.Setup(x => x.Execute<SitecoreItem>(It.IsAny<IRestRequest>())).Returns(topLevelResponse);

            restClient.Setup(x => x.Execute<List<SitecoreItem>>(It.IsAny<IRestRequest>()))
                                        .ReturnsInOrder(topLevelChildrenResponse, firstSubChildResponse, secondSubChildResponse);

            var serviceClient = new SitecoreItemServiceClient(restClient.Object, configurationParameters);

            // Act
            var urlList = serviceClient.GetSitecoreCmsTreeUrls().ToArray();

            // Assert
            Assert.IsNotNull(urlList);
            Assert.AreEqual(7, urlList.Length);
            Assert.AreEqual("/item/home", urlList[0].ExternalUrl);
            Assert.AreEqual("/item/url", urlList[1].ExternalUrl);
            Assert.AreEqual("/item/url/child1", urlList[2].ExternalUrl);
            Assert.AreEqual("/item/url/child2", urlList[3].ExternalUrl);
            Assert.AreEqual("/item/url2", urlList[4].ExternalUrl);
            Assert.AreEqual("/item/url2/child3", urlList[5].ExternalUrl);
            Assert.AreEqual("/item/url2/child4", urlList[6].ExternalUrl);
        }

        [Test]
        public void Service_client_handles_multiple_levels_of_children()
        {
            // Arrange
            var configurationParameters = new ConfigurationParameters
            {
                BaseUrl = "http://baseurl.com",
                RootNodeId = Guid.NewGuid()
            };

            var restClient = new Mock<IRestClient>();

            var rootItem = new SitecoreItem
            {
                DisplayName = "display name",
                HasChildren = true,
                ItemID = Guid.NewGuid(),
                ItemPath = "/path/to/item",
                ExternalUrl = "/item/root"
            };

            var rootResponse = new RestResponse<SitecoreItem> {Data = rootItem, StatusCode = HttpStatusCode.OK};

            var topLevelItemList = new List<SitecoreItem>
            {
                new SitecoreItem
                {
                    DisplayName = "display name",
                    HasChildren = true,
                    ItemID = Guid.NewGuid(),
                    ItemPath = "/path/to/item",
                    ExternalUrl = "/item/url"
                },
                new SitecoreItem
                {
                    DisplayName = "display name 2",
                    HasChildren = true,
                    ItemID = Guid.NewGuid(),
                    ItemPath = "/path/to/item2",
                    ExternalUrl = "/item/url2"
                }
            };

            var topLevelResponse = new RestResponse<List<SitecoreItem>> { Data = topLevelItemList, StatusCode = HttpStatusCode.OK };

            var firstChildItemList = new List<SitecoreItem>
            {
                new SitecoreItem
                {
                    DisplayName = "display name child 1",
                    HasChildren = false,
                    ItemID = Guid.NewGuid(),
                    ItemPath = "/path/to/item/child1",
                    ExternalUrl = "/item/url/child1"
                },
                new SitecoreItem
                {
                    DisplayName = "display name child 2",
                    HasChildren = true,
                    ItemID = Guid.NewGuid(),
                    ItemPath = "/path/to/item/child2",
                    ExternalUrl = "/item/url/child2"
                }
            };

            var firstChildResponse = new RestResponse<List<SitecoreItem>> { Data = firstChildItemList, StatusCode = HttpStatusCode.OK };

            var child2ChildrenItemList = new List<SitecoreItem>
            {
                new SitecoreItem
                {
                    DisplayName = "display name sub child 1",
                    HasChildren = false,
                    ItemID = Guid.NewGuid(),
                    ItemPath = "/path/to/item/child2/item1",
                    ExternalUrl = "/item/url/child2/subchild1"
                },
                new SitecoreItem
                {
                    DisplayName = "display name sub child 2",
                    HasChildren = false,
                    ItemID = Guid.NewGuid(),
                    ItemPath = "/path/to/item/child2/item2",
                    ExternalUrl = "/item/url/child2/subchild2"
                }
            };

            var child2ChildrenResponse = new RestResponse<List<SitecoreItem>> { Data = child2ChildrenItemList, StatusCode = HttpStatusCode.OK };

            var secondChildItemList = new List<SitecoreItem>
            {
                new SitecoreItem
                {
                    DisplayName = "display name child 3",
                    HasChildren = true,
                    ItemID = Guid.NewGuid(),
                    ItemPath = "/path/to/item/child1",
                    ExternalUrl = "/item/url2/child3"
                },
                new SitecoreItem
                {
                    DisplayName = "display name child 4",
                    HasChildren = false,
                    ItemID = Guid.NewGuid(),
                    ItemPath = "/path/to/item/child2",
                    ExternalUrl = "/item/url2/child4"
                }
            };

            var secondChildResponse = new RestResponse<List<SitecoreItem>> { Data = secondChildItemList, StatusCode = HttpStatusCode.OK };
            
            var child3ChildrenItemList = new List<SitecoreItem>
            {
                new SitecoreItem
                {
                    DisplayName = "display name sub child 1",
                    HasChildren = false,
                    ItemID = Guid.NewGuid(),
                    ItemPath = "/path/to/item/child3/item1",
                    ExternalUrl = "/item/url/child3/subchild1"
                },
                new SitecoreItem
                {
                    DisplayName = "display name sub child 2",
                    HasChildren = false,
                    ItemID = Guid.NewGuid(),
                    ItemPath = "/path/to/item/child3/item2",
                    ExternalUrl = "/item/url/child3/subchild2"
                }
            };

            var child3ChildrenResponse = new RestResponse<List<SitecoreItem>> { Data = child3ChildrenItemList, StatusCode = HttpStatusCode.OK };

            restClient.Setup(x => x.Execute<SitecoreItem>(It.IsAny<IRestRequest>())).Returns(rootResponse);

            restClient.Setup(x => x.Execute<List<SitecoreItem>>(It.IsAny<IRestRequest>()))
            .ReturnsInOrder(topLevelResponse, firstChildResponse, child2ChildrenResponse, secondChildResponse, child3ChildrenResponse);

            var serviceClient = new SitecoreItemServiceClient(restClient.Object, configurationParameters);

            // Act
            var urlList = serviceClient.GetSitecoreCmsTreeUrls().ToArray();

            // Assert
            Assert.IsNotNull(urlList);
            Assert.AreEqual(11, urlList.Length);
            Assert.AreEqual("/item/root", urlList[0].ExternalUrl);
            Assert.AreEqual("/item/url", urlList[1].ExternalUrl);
            Assert.AreEqual("/item/url/child1", urlList[2].ExternalUrl);
            Assert.AreEqual("/item/url/child2", urlList[3].ExternalUrl);
            Assert.AreEqual("/item/url/child2/subchild1", urlList[4].ExternalUrl);
            Assert.AreEqual("/item/url/child2/subchild2", urlList[5].ExternalUrl);                        
            Assert.AreEqual("/item/url2", urlList[6].ExternalUrl);
            Assert.AreEqual("/item/url2/child3", urlList[7].ExternalUrl);
            Assert.AreEqual("/item/url/child3/subchild1", urlList[8].ExternalUrl);
            Assert.AreEqual("/item/url/child3/subchild2", urlList[9].ExternalUrl);
            Assert.AreEqual("/item/url2/child4", urlList[10].ExternalUrl);
        }

        [Test]        
        public void Service_client_throws_exception_if_not_authorised()
        {
            // Arrange
            var configurationParameters = new ConfigurationParameters
            {
                BaseUrl = "http://baseurl.com",
                RootNodeId = Guid.NewGuid()
            };

            var restClient = new Mock<IRestClient>();

            var response = new RestResponse<SitecoreItem> { Data = null, StatusCode = HttpStatusCode.Forbidden };

            restClient.Setup(x => x.Execute<SitecoreItem>(It.IsAny<IRestRequest>())).Returns(response);

            var serviceClient = new SitecoreItemServiceClient(restClient.Object, configurationParameters);
            
            // Act / Assert
            Assert.Throws<ServiceClientAuthorizationException>(() => serviceClient.GetSitecoreCmsTreeUrls());
        }

        [Test]
        public void Service_client_handles_login_and_logout_if_credentials_specified()
        {
            // Arrange
            var configurationParameters = new ConfigurationParameters
            {
                BaseUrl = "http://baseurl.com",
                RootNodeId = Guid.NewGuid(),
                Domain = "sitecore",
                UserName = "username",
                Password = "password"
            };

            var restClient = new Mock<IRestClient>();

            var loginResponse = new RestResponse<LoginResult> { Data = new LoginResult(), StatusCode = HttpStatusCode.OK };

            restClient.Setup(x => x.Execute<LoginResult>(It.IsAny<IRestRequest>())).Returns(loginResponse).Verifiable();

            var rootNode = new SitecoreItem
            {
                DisplayName = "display name",
                HasChildren = false,
                ItemID = Guid.NewGuid(),
                ItemPath = "/path/to/item",
                ExternalUrl = "/item/root"
            };
            
            var rootResponse = new RestResponse<SitecoreItem> { Data = rootNode, StatusCode = HttpStatusCode.OK };

            restClient.Setup(x => x.Execute<SitecoreItem>(It.IsAny<IRestRequest>())).Returns(rootResponse);

            var response = new RestResponse<List<SitecoreItem>> { Data = new List<SitecoreItem>(), StatusCode = HttpStatusCode.OK };

            restClient.Setup(x => x.Execute<List<SitecoreItem>>(It.IsAny<IRestRequest>())).Returns(response);
            
            var logoutResponse = new RestResponse<LogoutResult> { Data = new LogoutResult(), StatusCode = HttpStatusCode.OK };

            restClient.Setup(x => x.Execute<LogoutResult>(It.IsAny<IRestRequest>())).Returns(logoutResponse).Verifiable();

            var serviceClient = new SitecoreItemServiceClient(restClient.Object, configurationParameters);

            // Act
            var sitecoreItems = serviceClient.GetSitecoreCmsTreeUrls(); 
            
            // Assert
            Assert.AreEqual(1, sitecoreItems.Count);   
            Assert.IsFalse(sitecoreItems[0].HasChildren);
            restClient.VerifyAll();
        }
    }
}
