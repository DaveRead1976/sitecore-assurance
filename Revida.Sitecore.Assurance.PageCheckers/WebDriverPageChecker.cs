using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace Revida.Sitecore.Assurance.PageCheckers
{
    public class WebDriverPageChecker : IPageChecker
    {
        private IWebDriver Driver { get; }

        public WebDriverPageChecker()
        {
            Driver = new FirefoxDriver();
        }

        public WebDriverPageChecker(IWebDriver driver)
        {
            Driver = driver;
        }

        public PageCheckResult PageResponseValid(Uri pageUrl)
        {
            Driver.Navigate().GoToUrl(pageUrl);

            IWebElement header = Driver.FindElement(By.TagName("head"));

            IWebElement body = Driver.FindElement(By.TagName("body"));

            if (header == null || body == null)
            {
                return new PageCheckResult {Success = false };
            }

            return new PageCheckResult { Success = true };
        }

        public void Close()
        {
            Driver.Close();
            Driver.Dispose();
        }
    }
}
