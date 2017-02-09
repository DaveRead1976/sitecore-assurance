using System.Net;

namespace Revida.Sitecore.Assurance.PageCheckers
{
    public class HttpWebRequestFactory : IHttpWebRequestFactory
    {
        public HttpWebRequest Create(string uri)
        {
            return WebRequest.CreateHttp(uri);
        }
    }
}
