using System;
using System.Diagnostics.CodeAnalysis;
using Moq;
using NUnit.Framework;
using OpenQA.Selenium;
using Revida.Sitecore.Assurance.PageCheckers;

namespace Revida.Sitecore.Assurance.Tests
{
    using System.Text;
    using Model;

    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class WebDriverPageCheckerTests
    {
        [Test]
        public void Web_driver_checker_inspects_page_and_returns_true_if_contains_head_and_body()
        {
            // Arrange
            var webDriver = new Mock<IWebDriver>();
            var navigation = new Mock<INavigation>();
            var options = new Mock<IOptions>();
            var window = new Mock<IWindow>();
            options.Setup(x => x.Window).Returns(window.Object);
            navigation.Setup(x => x.GoToUrl(It.IsAny<Uri>()));
            webDriver.Setup(x => x.Navigate()).Returns(navigation.Object);
            webDriver.Setup(x => x.Manage()).Returns(options.Object);
            webDriver.As<ITakesScreenshot>().Setup(x => x.GetScreenshot()).Returns(null as Screenshot);

            var element = new Mock<IWebElement>();
            webDriver.Setup(x => x.FindElement(It.IsAny<By>())).ReturnsInOrder(element.Object, element.Object);
            
            var pageChecker = new WebDriverPageChecker(webDriver.Object, "folder-name");

            // Act
            var status = pageChecker.PageResponseValid("http://baseurl", new SitecoreItem { ItemUrl = "/test", ItemPath = "/sitecore/test"});

            // Assert
            Assert.IsTrue(status.Success);
        }

        [Test]
        public void Web_driver_checker_fails_test_if_page_does_not_contain_head_element() 
        {
            // Arrange
            var webDriver = new Mock<IWebDriver>();
            var navigation = new Mock<INavigation>();
            var options = new Mock<IOptions>();
            var window = new Mock<IWindow>();
            options.Setup(x => x.Window).Returns(window.Object);
            navigation.Setup(x => x.GoToUrl(It.IsAny<Uri>()));
            webDriver.Setup(x => x.Navigate()).Returns(navigation.Object);
            webDriver.Setup(x => x.Manage()).Returns(options.Object);
            webDriver.As<ITakesScreenshot>().Setup(x => x.GetScreenshot()).Returns(null as Screenshot);

            var element = new Mock<IWebElement>();
            webDriver.Setup(x => x.FindElement(It.IsAny<By>())).ReturnsInOrder(null, element.Object);

            var pageChecker = new WebDriverPageChecker(webDriver.Object, "folder-name");

            // Act
            var status = pageChecker.PageResponseValid("http://baseurl", new SitecoreItem { ItemUrl = "/test", ItemPath = "/sitecore/test" });

            // Assert
            Assert.IsFalse(status.Success);
        }

        [Test]
        public void Web_driver_checker_fails_test_if_page_does_not_contain_body_element()
        {
            // Arrange
            var webDriver = new Mock<IWebDriver>();
            var navigation = new Mock<INavigation>();
            var options = new Mock<IOptions>();
            var window = new Mock<IWindow>();
            options.Setup(x => x.Window).Returns(window.Object);
            navigation.Setup(x => x.GoToUrl(It.IsAny<Uri>()));
            webDriver.Setup(x => x.Navigate()).Returns(navigation.Object);
            webDriver.Setup(x => x.Manage()).Returns(options.Object);
            webDriver.As<ITakesScreenshot>().Setup(x => x.GetScreenshot()).Returns(null as Screenshot);

            var element = new Mock<IWebElement>();
            webDriver.Setup(x => x.FindElement(It.IsAny<By>())).ReturnsInOrder(element.Object, null);

            var pageChecker = new WebDriverPageChecker(webDriver.Object, "folder-name");

            // Act
            var status = pageChecker.PageResponseValid("http://baseurl", new SitecoreItem { ItemUrl = "/test", ItemPath = "/sitecore/test" });

            // Assert
            Assert.IsFalse(status.Success);
        }
    }
}
