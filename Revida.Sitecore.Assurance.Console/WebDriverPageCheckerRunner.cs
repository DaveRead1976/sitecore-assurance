using System;
using System.Collections.Generic;
using Autofac;
using Revida.Sitecore.Assurance.Configuration;
using Revida.Sitecore.Assurance.PageCheckers;
using Revida.Sitecore.Services.Client;

namespace Revida.Sitecore.Assurance.Console
{

    public class WebDriverPageCheckerRunner
    {
        public void Run(ConfigurationParameters config, List<SitecoreItem> sitecoreItems)
        {
            WebDriverPageChecker checker = new WebDriverPageChecker();

            int index = 0;

            foreach (SitecoreItem sitecoreItem in sitecoreItems)
            {
                if (index == 0)
                {
                    System.Console.WriteLine("Success?\tItem path");
                }

                Uri pageUrl = new Uri($"{config.BaseUrl}/{sitecoreItem.ItemUrl}");

                PageCheckResult result = checker.PageResponseValid(pageUrl);

                System.Console.WriteLine($"{result?.Success}\t{sitecoreItem.ItemPath}");
                index++;
            }

            checker.Close();
        }
    }
}
