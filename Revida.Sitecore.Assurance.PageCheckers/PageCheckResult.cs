
using System.Net;

namespace Revida.Sitecore.Assurance.PageCheckers
{
    public class PageCheckResult
    {
        public bool Success { get; set; }

        public HttpStatusCode StatusCode { get; set; }
    }
}
