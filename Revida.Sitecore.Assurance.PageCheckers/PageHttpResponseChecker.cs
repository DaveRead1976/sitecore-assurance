
using System;
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

        public PageCheckResult PageResponseValid(Uri pageUrl)
        {
            try
            {
                var httpWebRequest = RequestFactory.Create(pageUrl.AbsoluteUri);
                httpWebRequest.AllowAutoRedirect = false;
                var response = (HttpWebResponse) httpWebRequest.GetResponse();

                bool success = (response.StatusCode == HttpStatusCode.OK ||
                        response.StatusCode == HttpStatusCode.Moved ||
                        response.StatusCode == HttpStatusCode.Redirect);

                return new PageCheckResult {Success = success, StatusCode = response.StatusCode};
            }
            catch (WebException webException)
            {
                if (webException.Status == WebExceptionStatus.Timeout)
                {
                    return new PageCheckResult {Success = false, StatusCode = HttpStatusCode.RequestTimeout};
                }

                HttpWebResponse exceptionResponse = (HttpWebResponse) webException.Response;

                return new PageCheckResult { Success = false, StatusCode = exceptionResponse.StatusCode };
            }
        }
    }
}
 