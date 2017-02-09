
using System.Net;

namespace Revida.Sitecore.Assurance.PageCheckers
{
    public class PageHttpResponseChecker
    {
        private IHttpWebRequestFactory RequestFactory { get; }

        public PageHttpResponseChecker(IHttpWebRequestFactory requestFactory)
        {
            RequestFactory = requestFactory;
        }

        public bool PageResponseValid(string pageUrl)
        {
            var httpWebRequest = RequestFactory.Create(pageUrl);
            httpWebRequest.AllowAutoRedirect = false;
            var response = (HttpWebResponse)httpWebRequest.GetResponse();

            return (response.StatusCode == HttpStatusCode.OK || 
                    response.StatusCode == HttpStatusCode.Moved || 
                    response.StatusCode == HttpStatusCode.Redirect);
        }
    }
}
 