using System.Collections.Generic;
using System.IO;
using Autofac;
using Revida.Sitecore.Assurance.Configuration;
using Revida.Sitecore.Assurance.Model;
using Revida.Sitecore.Assurance.PageCheckers;

namespace Revida.Sitecore.Assurance.Console
{
    public class HttpPageCheckerRunner : RunnerBase
    {
        private const string OutputFilename = "http-status-code-test-results";

        private IContainer Container { get; }

        public HttpPageCheckerRunner(IContainer container)
        {
            Container = container;
        }

        public void Run(ConfigurationParameters config, List<SitecoreItem> sitecoreItems)
        {
            if (sitecoreItems.Count == 0)
            {
                return;
            }

            FileStream outputFile = CreateOutputFile(OutputFilename);
            var writer = new StreamWriter(outputFile);

            writer.WriteLine("Full URL,Success?,Status Code,Item path");

            var checker = Container.Resolve<HttpResponsePageChecker>();

            foreach (SitecoreItem sitecoreItem in sitecoreItems)
            {                
                var result = checker.PageResponseValid(config.BaseUrl, sitecoreItem) as HttpPageCheckResult;

                writer.WriteLine($"{config.BaseUrl}{sitecoreItem.ExternalUrl},{result?.Success},{result?.StatusCode},{sitecoreItem.ItemPath}");
            }

            writer.Flush();
            writer.Close();
        }
    }
}
