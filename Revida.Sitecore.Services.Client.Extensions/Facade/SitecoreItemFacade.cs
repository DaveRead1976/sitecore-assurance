using System.Diagnostics.CodeAnalysis;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Links;

namespace Revida.Sitecore.Services.Client.Extensions.Facade
{    
    [ExcludeFromCodeCoverage]
    public class SitecoreItemFacade : ISitecoreItemFacade
    {
        public string GetItemExternalUrl(Item item)
        {
            return LinkManager.GetItemUrl(item);
        }

        public Item GetItem(ID id)
        {
            return global::Sitecore.Context.Database.GetItem(id);
        }
    }
}
