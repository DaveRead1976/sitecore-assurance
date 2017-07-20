using Sitecore.Data;
using Sitecore.Data.Items;

namespace Revida.Sitecore.Services.Client.Extensions.Facade
{    
    public interface ISitecoreItemFacade
    {
        string GetItemExternalUrl(Item item);

        Item GetItem(ID id);
    }
}
