﻿using System;
using System.Diagnostics.CodeAnalysis;
using Moq;
using NUnit.Framework;
using OpenQA.Selenium;
using Revida.Sitecore.Assurance.PageCheckers;

namespace Revida.Sitecore.Assurance.Tests
{
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
            navigation.Setup(x => x.GoToUrl(It.IsAny<Uri>()));
            webDriver.Setup(x => x.Navigate()).Returns(navigation.Object);

            var element = new Mock<IWebElement>();
            webDriver.Setup(x => x.FindElement(It.IsAny<By>())).ReturnsInOrder(element.Object, element.Object);
            
            var pageChecker = new WebDriverPageChecker(webDriver.Object);

            // Act
            var status = pageChecker.PageResponseValid(new Uri("http://test.com"));

            // Assert
            Assert.IsTrue(status.Success);
        }

        [Test]
        public void Web_driver_checker_fails_test_if_page_does_not_contain_head_element() 
        {
            // Arrange
            var webDriver = new Mock<IWebDriver>();
            var navigation = new Mock<INavigation>();
            navigation.Setup(x => x.GoToUrl(It.IsAny<Uri>()));
            webDriver.Setup(x => x.Navigate()).Returns(navigation.Object);

            var element = new Mock<IWebElement>();
            webDriver.Setup(x => x.FindElement(It.IsAny<By>())).ReturnsInOrder(null, element.Object);

            var pageChecker = new WebDriverPageChecker(webDriver.Object);

            // Act
            var status = pageChecker.PageResponseValid(new Uri("http://test.com"));

            // Assert
            Assert.IsFalse(status.Success);
        }

        [Test]
        public void Web_driver_checker_fails_test_if_page_does_not_contain_body_element()
        {
            // Arrange
            var webDriver = new Mock<IWebDriver>();
            var navigation = new Mock<INavigation>();
            navigation.Setup(x => x.GoToUrl(It.IsAny<Uri>()));
            webDriver.Setup(x => x.Navigate()).Returns(navigation.Object);

            var element = new Mock<IWebElement>();
            webDriver.Setup(x => x.FindElement(It.IsAny<By>())).ReturnsInOrder(element.Object, null);

            var pageChecker = new WebDriverPageChecker(webDriver.Object);

            // Act
            var status = pageChecker.PageResponseValid(new Uri("http://test.com"));

            // Assert
            Assert.IsFalse(status.Success);
        }
    }
}
