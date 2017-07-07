using System;
using System.Net;

namespace Revida.Sitecore.Assurance.PageCheckers
{
    public class HttpResponsePageChecker : IPageChecker
    {
        private IHttpWebRequestFactory RequestFactory { get; }

        public HttpResponsePageChecker(IHttpWebRequestFactory requestFactory)
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

                return new HttpPageCheckResult { Success = success, StatusCode = response.StatusCode};
            }
            catch (WebException webException)
            {
                if (webException.Status == WebExceptionStatus.Timeout)
                {
                    return new HttpPageCheckResult { Success = false, StatusCode = HttpStatusCode.RequestTimeout};
                }

                HttpWebResponse exceptionResponse = (HttpWebResponse) webException.Response;

                return new HttpPageCheckResult { Success = false, StatusCode = exceptionResponse.StatusCode };
            }
        }
    }
}
 