using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace Revida.Sitecore.Assurance.PageCheckers
{
    [ExcludeFromCodeCoverage]
    public class HttpWebRequestFactory : IHttpWebRequestFactory
    {
        public HttpWebRequest Create(string uri)
        {
            return WebRequest.CreateHttp(uri);
        }
    }
}
