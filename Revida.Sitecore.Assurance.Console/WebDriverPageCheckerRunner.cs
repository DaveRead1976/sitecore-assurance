using System;
using System.IO;
using System.Collections.Generic;
using Revida.Sitecore.Assurance.Configuration;
using Revida.Sitecore.Assurance.Model;
using Revida.Sitecore.Assurance.PageCheckers;

namespace Revida.Sitecore.Assurance.Console
{
    public class WebDriverPageCheckerRunner : RunnerBase
    {
        private const string OutputFilename = "webdriver-test-results";

        public void Run(ConfigurationParameters config, List<SitecoreItem> sitecoreItems)
        {
            string screenShotsFolder = CreateScreenShotDirectory();

            var checker = new WebDriverPageChecker(screenShotsFolder);

            FileStream outputFile = CreateOutputFile(OutputFilename);
            StreamWriter writer = new StreamWriter(outputFile);
            int index = 0;
            
            foreach (SitecoreItem sitecoreItem in sitecoreItems)
            {
                if (index == 0)
                {
                    writer.WriteLine("Full URL,Success?,Item path");
                }

                PageCheckResult result = checker.PageResponseValid(config.BaseUrl, sitecoreItem);

                writer.WriteLine($"{config.BaseUrl}{sitecoreItem.ExternalUrl},{result?.Success},{sitecoreItem.ItemPath}");
                index++;
            }

            checker.Close();

            writer.Flush();
            writer.Close();
        }

        private string CreateScreenShotDirectory()
        {
            string screenshotsFolder = $"{AppDomain.CurrentDomain.BaseDirectory}\\ScreenShots";

            if (!Directory.Exists(screenshotsFolder))
            {
                Directory.CreateDirectory(screenshotsFolder);
            }

            DateTime now = DateTime.Now;
            string timeStamp = $"{now.Year}{now.Month.ToString("00")}{now.Day.ToString("00")}{now.Hour.ToString("00")}{now.Minute.ToString("00")}{now.Second.ToString("00")}";

            string screenshotsFolderForCurrentRun = $"{screenshotsFolder}\\{timeStamp}";

            Directory.CreateDirectory(screenshotsFolderForCurrentRun);

            return screenshotsFolderForCurrentRun;
        }
    }
}
