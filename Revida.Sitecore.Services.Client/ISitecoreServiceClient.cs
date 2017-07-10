using System.Collections.Generic;
using Revida.Sitecore.Assurance.Model;

namespace Revida.Sitecore.Services.Client
{
    public interface ISitecoreServiceClient
    {
        List<SitecoreItem> GetSitecoreCmsTreeUrls();
    }
}
