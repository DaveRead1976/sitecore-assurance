using System;
using System.Collections.Generic;
using Autofac;
using Revida.Sitecore.Assurance.Configuration;
using Revida.Sitecore.Assurance.Model;
using Revida.Sitecore.Assurance.PageCheckers;
using Revida.Sitecore.Services.Client;

namespace Revida.Sitecore.Assurance.Console
{
    public class Program
    {
        private static IContainer Container { get; set; }
        
        public static void Main(string[] args)
        {
            RegisterIocModules();

            ConfigurationParameters config;
            if (!ConfigurationFileHelper.LoadConfiguration(args, out config))
            {
                ShowUsage();
                return;
            }

            List<SitecoreItem> sitecoreItems;

            if (!string.IsNullOrWhiteSpace(config.InputFileName))
            {
                System.Console.WriteLine($"Loading URL list from {config.InputFileName}");

                var inputFileUrlListRunner = new InputFileUrlListRunner(config);
                sitecoreItems = inputFileUrlListRunner.Run();
            }
            else
            {
                System.Console.WriteLine($"Root Node GUID: {config.RootNodeId}");

                var siteTreeTraversalRunner = new SiteTreeTraversalRunner(Container, config);
                sitecoreItems = siteTreeTraversalRunner.Run();
            }

            System.Console.WriteLine($"{sitecoreItems.Count} Sitecore URLs in content tree" );

            if (config.ListUrls && string.IsNullOrWhiteSpace(config.InputFileName))
            {
                var listItemsRunner = new UrlListingRunner(config);
                listItemsRunner.Run(sitecoreItems);                
                return;
            }

            if (config.RunHttpChecker)
            {
                var httpRunner = new HttpPageCheckerRunner(Container);
                httpRunner.Run(config, sitecoreItems);
            }
            if (config.RunWebDriverChecker)
            {
                var webDriverRunner = new WebDriverPageCheckerRunner();
                webDriverRunner.Run(config, sitecoreItems);
            }

            Environment.Exit(0);
        }

        private static void RegisterIocModules()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<PageCheckersModule>();
            builder.RegisterModule<ServicesClientModule>();
            Container = builder.Build();
        }

        private static void ShowUsage()
        {
            System.Console.WriteLine("Usage: sitecore-assurance -r {root node guid} -b {base url} [-u {user name}] [-p {password}] [-d {domain}] [-l] [-h] [-s] ");
            System.Console.WriteLine("       sitecore-assurance --root {root node guid} --baseurl {base url} [--username {user name}] [--password {password}] [--domain {domain}] [--list] [--http] [--selenium]");
        }
    }
}
