using System;
using Revida.Sitecore.Assurance.Model;

namespace Revida.Sitecore.Assurance.PageCheckers
{
    public abstract class BasePageChecker
    {
        protected Uri GeneratePageUrl(string baseUrl, SitecoreItem sitecoreItem)
        {
            return new Uri($"{baseUrl}/{sitecoreItem.ExternalUrl}");
        }
    }
}
