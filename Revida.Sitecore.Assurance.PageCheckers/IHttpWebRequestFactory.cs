using System.Net;

namespace Revida.Sitecore.Assurance.PageCheckers
{
    public interface IHttpWebRequestFactory
    {
        HttpWebRequest Create(string uri);
    }
}
