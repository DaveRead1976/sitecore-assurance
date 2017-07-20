using System.Collections.Generic;
using System.IO;
using Revida.Sitecore.Assurance.Configuration;
using Revida.Sitecore.Assurance.Model;

namespace Revida.Sitecore.Assurance.Console
{
    public class UrlListingRunner : RunnerBase
    {
        private const string OutputFilename = "list-urls-results";

        private ConfigurationParameters ConfigurationParameters { get; }

        public UrlListingRunner(ConfigurationParameters configurationParameters)
        {
            ConfigurationParameters = configurationParameters;
        }

        public void Run(List<SitecoreItem> sitecoreItems)
        {
            if (sitecoreItems.Count > 0)
            {
                FileStream outputFile = CreateOutputFile(OutputFilename);
                StreamWriter writer = new StreamWriter(outputFile);
                
                writer.WriteLine("Full URL,Item path,Item ID,Item Name,Has Children,Display Name,Relative URL");

                foreach (SitecoreItem sitecoreItem in sitecoreItems)
                {
                    writer.WriteLine($"{ConfigurationParameters.BaseUrl}{sitecoreItem.ExternalUrl},{sitecoreItem.ItemPath},{sitecoreItem.ItemID},{sitecoreItem.ItemName},{sitecoreItem.HasChildren},{sitecoreItem.DisplayName},{sitecoreItem.ExternalUrl}");
                }
                writer.Flush();
                writer.Close();
                System.Console.WriteLine($"Output written to {outputFile.Name}");
            }
            else
            {
                System.Console.WriteLine("No sitecore URLs found");
            }
        }        
    }
}
