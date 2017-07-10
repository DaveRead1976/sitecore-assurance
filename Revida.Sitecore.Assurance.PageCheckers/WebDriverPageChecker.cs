using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using Revida.Sitecore.Assurance.Model;

namespace Revida.Sitecore.Assurance.PageCheckers
{
    public class WebDriverPageChecker : BasePageChecker, IPageChecker
    {
        private IWebDriver Driver { get; }

        private string ScreenShotsFolder { get; }

        public WebDriverPageChecker(string screenShotsFolder)
        {
            Driver = new FirefoxDriver();
            ScreenShotsFolder = screenShotsFolder;
        }

        public WebDriverPageChecker(IWebDriver driver, string screenShotsFolder)
        {
            Driver = driver;
            ScreenShotsFolder = screenShotsFolder;
        }

        public PageCheckResult PageResponseValid(string baseUrl, SitecoreItem sitecoreItem)
        {
            Uri pageUrl = GeneratePageUrl(baseUrl, sitecoreItem);
            Driver.Navigate().GoToUrl(pageUrl);
            Driver.Manage().Window.Maximize();

            IWebElement header = Driver.FindElement(By.TagName("head"));

            IWebElement body = Driver.FindElement(By.TagName("body"));

            Screenshot screenShot = ((ITakesScreenshot)Driver).GetScreenshot();
            if (screenShot != null)
            {
                screenShot.SaveAsFile(GenerateScreenShotFileName(sitecoreItem), ScreenshotImageFormat.Png);
            }

            if (header == null || body == null)
            {
                return new PageCheckResult {Success = false };
            }

            return new PageCheckResult { Success = true };
        }

        private string GenerateScreenShotFileName(SitecoreItem sitecoreItem)
        {
            string fileName = sitecoreItem.ItemPath.Replace('/', '-'); 
            return $"{ScreenShotsFolder}\\{fileName}.png";            
        }

        public void Close()
        {
            Driver.Close();
            Driver.Dispose();
        }
    }
}
