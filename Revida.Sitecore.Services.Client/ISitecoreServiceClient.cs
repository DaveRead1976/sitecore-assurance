using System.Collections.Generic;

namespace Revida.Sitecore.Services.Client
{
    public interface ISitecoreServiceClient
    {
        List<SitecoreItem> GetSitecoreCmsTreeUrls();
    }
}
