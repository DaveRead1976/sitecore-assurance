using System;
using System.Collections.Generic;
using System.IO;
using Revida.Sitecore.Assurance.Configuration;
using Revida.Sitecore.Assurance.Model;

namespace Revida.Sitecore.Assurance.Console
{
    public class InputFileUrlListRunner : RunnerBase
    {
        private ConfigurationParameters Config { get; }

        public InputFileUrlListRunner(ConfigurationParameters config)
        {            
            Config = config;
        }

        public List<SitecoreItem> Run()
        {
            var sitecoreItemList = new List<SitecoreItem>();

            var fileStream = File.OpenRead(Config.InputFileName);

            var reader = new StreamReader(fileStream);

            reader.ReadLine();  // Skip headers

            string line = string.Empty;
            while ((line = reader.ReadLine()) != null)
            {
                string[] segments = line.Split(',');
                var sitecoreItem = new SitecoreItem
                {
                    ItemPath = segments[1],
                    ItemID = new Guid(segments[2]),
                    ItemName = segments[3],
                    HasChildren = Convert.ToBoolean(segments[4]),
                    DisplayName = segments[5],
                    ExternalUrl = segments[6]
                };
                sitecoreItemList.Add(sitecoreItem);
            }

            return sitecoreItemList;
        }
    }
}
