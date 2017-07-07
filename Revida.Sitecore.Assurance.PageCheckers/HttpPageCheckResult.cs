using System.Net;

namespace Revida.Sitecore.Assurance.PageCheckers
{
    public class HttpPageCheckResult : PageCheckResult
    {
        public HttpStatusCode StatusCode { get; set; }
    }
}
