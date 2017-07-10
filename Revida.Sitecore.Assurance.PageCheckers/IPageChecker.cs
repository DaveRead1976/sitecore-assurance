using Revida.Sitecore.Assurance.Model;

namespace Revida.Sitecore.Assurance.PageCheckers
{
    public interface IPageChecker
    {
        PageCheckResult PageResponseValid(string baseUrl, SitecoreItem sitecoreItem);
    }
}
