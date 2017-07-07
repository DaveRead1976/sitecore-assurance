
using System;

namespace Revida.Sitecore.Assurance.PageCheckers
{
    public interface IPageChecker
    {
        PageCheckResult PageResponseValid(Uri pageUrl);
    }
}
