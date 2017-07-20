using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Services.Core.Model;
using Revida.Sitecore.Services.Client.Extensions.Facade;

namespace Revida.Sitecore.Services.Client.Extensions.Filters
{    
    public class ItemModelActionFilter : ActionFilterAttribute
    {
        private ISitecoreItemFacade _sitecoreItem;

        public ISitecoreItemFacade SitecoreItemFacade
        {
            get { return _sitecoreItem ?? (_sitecoreItem = new SitecoreItemFacade()); }
            set { _sitecoreItem = value; }
        }

        public override Task OnActionExecutedAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            var objectContent = actionExecutedContext.Response.Content as ObjectContent;
            if (objectContent?.Value == null)
            {
                return base.OnActionExecutedAsync(actionExecutedContext, cancellationToken);
            }

            if (typeof(ItemModel).IsAssignableFrom(objectContent.ObjectType))
            {
                AddExternalUrlToModel(objectContent.Value as ItemModel);
            }
            else
            {
                if (IsArrayOfObjectType(objectContent))
                {
                    foreach (ItemModel model in (IEnumerable) objectContent.Value)
                    {
                        AddExternalUrlToModel(model);
                    }
                }
            }
            return base.OnActionExecutedAsync(actionExecutedContext, cancellationToken);
        }
        
        private void AddExternalUrlToModel(ItemModel model)
        {
            if (model == null)
            {
                return;
            }
            var itemGuid = new Guid(model["ItemID"].ToString());
            Item sitecoreItem = SitecoreItemFacade.GetItem(new ID(itemGuid));
            if (sitecoreItem == null)
            {
                return;
            }

            if (!model.ContainsKey("ExternalUrl"))
            {
                model.Add("ExternalUrl", SitecoreItemFacade.GetItemExternalUrl(sitecoreItem));
            }
        }


        private static bool IsArrayOfObjectType(ObjectContent objectContent)
        {
            return (typeof(IEnumerable<ItemModel>).IsAssignableFrom(objectContent.ObjectType)               
                    || (objectContent.ObjectType.IsArray && typeof(ItemModel).IsAssignableFrom(objectContent.ObjectType.GetElementType())));
        }
    }
}
