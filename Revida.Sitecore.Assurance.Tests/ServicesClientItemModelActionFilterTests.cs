using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Moq;
using NUnit.Framework;
using Revida.Sitecore.Services.Client.Extensions.Facade;
using Revida.Sitecore.Services.Client.Extensions.Filters;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Globalization;
using Sitecore.Services.Core.Model;

namespace Revida.Sitecore.Assurance.Tests
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class ServicesClientItemModelActionFilterTests
    {
        [Test]
        public void Action_filter_ignores_object_content_with_null_value()
        {
            // Arrange
            var facade = new Mock<ISitecoreItemFacade>();

            var filter = new ItemModelActionFilter { SitecoreItemFacade = facade.Object };
            
            var content = new ObjectContent(typeof(string), null, new JsonMediaTypeFormatter());
            var message = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = content
            };
            var actionContext = new HttpActionContext();
            var actionExecutedContext = new HttpActionExecutedContext
            {
                ActionContext = actionContext,
                Response = message
            };
            var token = new CancellationToken();

            // Act
            var taskResult = filter.OnActionExecutedAsync(actionExecutedContext, token);
            taskResult.Wait(token);

            // Assert
            Assert.IsNotNull(actionExecutedContext.ActionContext.Response.Content);            
        }

        [Test]
        public void Action_filter_ignores_object_content_with_reference_to_invalid_item_id()
        {
            // Arrange
            var facade = new Mock<ISitecoreItemFacade>();

            Item nullItem = null;
            facade.Setup(x => x.GetItem(It.IsAny<ID>())).Returns(nullItem);
            
            var filter = new ItemModelActionFilter { SitecoreItemFacade = facade.Object };

            var itemModel = new ItemModel
            {
                ["ItemID"] = Guid.NewGuid().ToString()
            };

            var content = new ObjectContent(typeof(ItemModel), itemModel, new JsonMediaTypeFormatter());
            var message = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = content
            };
            var actionContext = new HttpActionContext();
            var actionExecutedContext = new HttpActionExecutedContext
            {
                ActionContext = actionContext,
                Response = message
            };
            var token = new CancellationToken();

            // Act
            var taskResult = filter.OnActionExecutedAsync(actionExecutedContext, token);
            taskResult.Wait(token);

            // Assert
            Assert.IsNotNull(actionExecutedContext.ActionContext.Response.Content);
        }

        [Test]
        public void Action_filter_applies_external_url_to_single_model()
        {
            // Arrange
            var facade = new Mock<ISitecoreItemFacade>();

            var item = GenerateTestItem(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "services client item");
            facade.Setup(x => x.GetItem(It.IsAny<ID>())).Returns(item);
            facade.Setup(x => x.GetItemExternalUrl(It.IsAny<Item>())).Returns("http://itemurl.com");

            var filter = new ItemModelActionFilter { SitecoreItemFacade = facade.Object };

            var itemModel = new ItemModel
            {
                ["ItemID"] = Guid.NewGuid().ToString()
            };

            var content = new ObjectContent(typeof(ItemModel), itemModel, new JsonMediaTypeFormatter());
            var message = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = content
            };
            var actionContext = new HttpActionContext();
            var actionExecutedContext = new HttpActionExecutedContext
            {
                ActionContext = actionContext,
                Response = message
            };
            var token = new CancellationToken();

            // Act
            var taskResult = filter.OnActionExecutedAsync(actionExecutedContext, token);
            taskResult.Wait(token);

            // Assert
            Assert.IsNotNull(actionExecutedContext.ActionContext.Response.Content);
            var contentItemModel = ((ObjectContent) actionExecutedContext.ActionContext.Response.Content).Value as ItemModel;
            Assert.IsNotNull(contentItemModel);
            Assert.AreEqual("http://itemurl.com", contentItemModel["ExternalUrl"]);
        }

        [Test]
        public void Action_filter_applies_external_url_to_list_of_models()
        {
            // Arrange
            var facade = new Mock<ISitecoreItemFacade>();

            var item = GenerateTestItem(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "services client item");
            facade.Setup(x => x.GetItem(It.IsAny<ID>())).Returns(item);
            facade.Setup(x => x.GetItemExternalUrl(It.IsAny<Item>()))
                .ReturnsInOrder("http://itemurl.com/item1", "http://itemurl.com/item2", "http://itemurl.com/item3");

            var filter = new ItemModelActionFilter { SitecoreItemFacade = facade.Object };

            var itemModelList = new List<ItemModel>
            {
                new ItemModel
                { 
                    ["ItemID"] = Guid.NewGuid().ToString()
                },
                new ItemModel
                {
                    ["ItemID"] = Guid.NewGuid().ToString()
                },
                new ItemModel
                {
                    ["ItemID"] = Guid.NewGuid().ToString()
                }
            };

            var content = new ObjectContent(typeof(List<ItemModel>), itemModelList, new JsonMediaTypeFormatter());
            var message = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = content
            };
            var actionContext = new HttpActionContext();
            var actionExecutedContext = new HttpActionExecutedContext
            {
                ActionContext = actionContext,
                Response = message
            };
            var token = new CancellationToken();

            // Act
            var taskResult = filter.OnActionExecutedAsync(actionExecutedContext, token);
            taskResult.Wait(token);

            // Assert
            Assert.IsNotNull(actionExecutedContext.ActionContext.Response.Content);
            var contentItemModelList = ((ObjectContent)actionExecutedContext.ActionContext.Response.Content).Value as List<ItemModel>;
            Assert.IsNotNull(contentItemModelList);
            Assert.AreEqual(3, contentItemModelList.Count);
            Assert.AreEqual("http://itemurl.com/item1", contentItemModelList[0]["ExternalUrl"]);
            Assert.AreEqual("http://itemurl.com/item2", contentItemModelList[1]["ExternalUrl"]);
            Assert.AreEqual("http://itemurl.com/item3", contentItemModelList[2]["ExternalUrl"]);
        }

        [Test]
        public void Action_filter_applies_external_url_to_array_of_models()
        {
            // Arrange
            var facade = new Mock<ISitecoreItemFacade>();

            var item = GenerateTestItem(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "services client item");
            facade.Setup(x => x.GetItem(It.IsAny<ID>())).Returns(item);
            facade.Setup(x => x.GetItemExternalUrl(It.IsAny<Item>()))
                .ReturnsInOrder("http://itemurl.com/item1", "http://itemurl.com/item2", "http://itemurl.com/item3");

            var filter = new ItemModelActionFilter { SitecoreItemFacade = facade.Object };

            var itemModelArray = new ItemModel[3];

            itemModelArray[0] = new ItemModel
            {
                ["ItemID"] = Guid.NewGuid().ToString()
            };
            itemModelArray[1] = new ItemModel
            {
                ["ItemID"] = Guid.NewGuid().ToString()
            };
            itemModelArray[2] = new ItemModel
            {
                ["ItemID"] = Guid.NewGuid().ToString()
            };

            var content = new ObjectContent(typeof(ItemModel[]), itemModelArray, new JsonMediaTypeFormatter());
            var message = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = content
            };
            var actionContext = new HttpActionContext();
            var actionExecutedContext = new HttpActionExecutedContext
            {
                ActionContext = actionContext,
                Response = message
            };
            var token = new CancellationToken();

            // Act
            var taskResult = filter.OnActionExecutedAsync(actionExecutedContext, token);
            taskResult.Wait(token);

            // Assert
            Assert.IsNotNull(actionExecutedContext.ActionContext.Response.Content);
            var contentItemModelList = ((ObjectContent)actionExecutedContext.ActionContext.Response.Content).Value as ItemModel[];
            Assert.IsNotNull(contentItemModelList);
            Assert.AreEqual(3, contentItemModelList.Length);
            Assert.AreEqual("http://itemurl.com/item1", contentItemModelList[0]["ExternalUrl"]);
            Assert.AreEqual("http://itemurl.com/item2", contentItemModelList[1]["ExternalUrl"]);
            Assert.AreEqual("http://itemurl.com/item3", contentItemModelList[2]["ExternalUrl"]);
        }

        private static Item GenerateTestItem(Guid itemId, Guid templateId, Guid branchId, string itemName)
        {
            var actualItemId = new ID(itemId);
            var database = FormatterServices.GetUninitializedObject(typeof(Database)) as Database;
            var itemDefinition = new ItemDefinition(actualItemId, itemName, new ID(templateId), new ID(branchId));
            var itemData = new ItemData(itemDefinition, Language.Parse("en"), global::Sitecore.Data.Version.Parse(1), new FieldList());
            var item = new Item(actualItemId, itemData, database);
            return item;
        }
    }
}
