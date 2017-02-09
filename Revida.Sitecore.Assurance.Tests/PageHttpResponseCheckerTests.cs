using System.Net;
using Moq;
using NUnit.Framework;
using Revida.Sitecore.Assurance.PageCheckers;

namespace Revida.Sitecore.Assurance.Tests
{
    [TestFixture]
    public class PageHttpResponseCheckerTests
    {
        [TestCase(HttpStatusCode.OK, true)]
        [TestCase(HttpStatusCode.MovedPermanently, true)]
        [TestCase(HttpStatusCode.Redirect, true)]
        [TestCase(HttpStatusCode.BadRequest, false)]
        [TestCase(HttpStatusCode.Forbidden, false)]
        [TestCase(HttpStatusCode.NotFound, false)]
        [TestCase(HttpStatusCode.ProxyAuthenticationRequired, false)]
        [TestCase(HttpStatusCode.InternalServerError, false)]
        [TestCase(HttpStatusCode.ServiceUnavailable, false)]
        public void Page_status_code_responses_map_to_page_is_valid(HttpStatusCode statusCode, bool expectedIsValid)
        {
            // Arrange
            var request = new Mock<HttpWebRequest>();
            var response = new Mock<HttpWebResponse>();

            response.SetupGet(x => x.StatusCode).Returns(statusCode);
            request.Setup(x => x.GetResponse()).Returns(response.Object);
            
            var factory = new Mock<IHttpWebRequestFactory>();
            factory.Setup(x => x.Create(It.IsAny<string>())).Returns(request.Object);

            var pageChecker = new PageHttpResponseChecker(factory.Object);

            // Act
            var isValid = pageChecker.PageResponseValid("http://test.com");

            // Assert
            Assert.AreEqual(expectedIsValid, isValid);
        }
    }
}
