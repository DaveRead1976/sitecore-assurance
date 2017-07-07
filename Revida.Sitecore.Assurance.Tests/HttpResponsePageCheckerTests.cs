using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using Moq;
using NUnit.Framework;
using Revida.Sitecore.Assurance.PageCheckers;

namespace Revida.Sitecore.Assurance.Tests
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class HttpResponsePageCheckerTests
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

            var pageChecker = new HttpResponsePageChecker(factory.Object);

            // Act
            var validationResult = pageChecker.PageResponseValid(new Uri("http://test.com")) as HttpPageCheckResult;

            // Assert
            Assert.IsNotNull(validationResult);
            Assert.AreEqual(expectedIsValid, validationResult.Success);
            Assert.AreEqual(statusCode, validationResult.StatusCode);
        }

        [Test]
        public void Page_timeout_maps_to_correct_status_code()
        {
            // Arrange
            var request = new Mock<HttpWebRequest>();
            
            var webException = new WebException("Unit test exception", WebExceptionStatus.Timeout);

            request.Setup(x => x.GetResponse()).Throws(webException);

            var factory = new Mock<IHttpWebRequestFactory>();
            factory.Setup(x => x.Create(It.IsAny<string>())).Returns(request.Object);

            var pageChecker = new HttpResponsePageChecker(factory.Object);

            // Act
            var validationResult = pageChecker.PageResponseValid(new Uri("http://test.com")) as HttpPageCheckResult;

            // Assert
            Assert.IsNotNull(validationResult);
            Assert.IsFalse(validationResult.Success);
            Assert.AreEqual(HttpStatusCode.RequestTimeout, validationResult.StatusCode);
        }

        [Test]
        public void Page_web_exception_maps_to_correct_status_code()
        {
            // Arrange
            var request = new Mock<HttpWebRequest>();
            var response = new Mock<HttpWebResponse>();

            response.SetupGet(x => x.StatusCode).Returns(HttpStatusCode.NotFound);
            var innerException = new Exception("Inner exception");

            var webException = new WebException("Unit test exception", innerException, WebExceptionStatus.ProtocolError, response.Object);

            request.Setup(x => x.GetResponse()).Throws(webException);

            var factory = new Mock<IHttpWebRequestFactory>();
            factory.Setup(x => x.Create(It.IsAny<string>())).Returns(request.Object);

            var pageChecker = new HttpResponsePageChecker(factory.Object);

            // Act
            var validationResult = pageChecker.PageResponseValid(new Uri("http://test.com")) as HttpPageCheckResult;

            // Assert
            Assert.IsNotNull(validationResult);
            Assert.IsFalse(validationResult.Success);
            Assert.AreEqual(HttpStatusCode.NotFound, validationResult.StatusCode);
        }
    }
}
