using System.Collections.Generic;
using Autofac;
using Revida.Sitecore.Assurance.Configuration;
using Revida.Sitecore.Assurance.Model;
using Revida.Sitecore.Assurance.PageCheckers;

namespace Revida.Sitecore.Assurance.Console
{
    public class HttpPageCheckerRunner
    {
        private IContainer Container { get; }

        public HttpPageCheckerRunner(IContainer container)
        {
            Container = container;
        }

        public void Run(ConfigurationParameters config, List<SitecoreItem> sitecoreItems)
        {
            if (sitecoreItems.Count > 0)
            {
                System.Console.WriteLine("Success?\tStatus Code\tItem path");
            }

            HttpResponsePageChecker checker = Container.Resolve<HttpResponsePageChecker>();

            foreach (SitecoreItem sitecoreItem in sitecoreItems)
            {                
                HttpPageCheckResult result = checker.PageResponseValid(config.BaseUrl, sitecoreItem) as HttpPageCheckResult;

                System.Console.WriteLine($"{result?.Success}\t{result?.StatusCode}\t{sitecoreItem.ItemPath}");
            }
        }
    }
}
