using System;
using System.Collections.Generic;
using Autofac;
using RestSharp;
using Revida.Sitecore.Assurance.Configuration;
using Revida.Sitecore.Assurance.Model;
using Revida.Sitecore.Assurance.PageCheckers;
using Revida.Sitecore.Services.Client;

namespace Revida.Sitecore.Assurance.Console
{

    public class Program
    {
        private static IContainer Container { get; set; }

        private static ConfigurationParameters Config { get; set; }

        public static void Main(string[] args)
        {
            RegisterIocModules();
            try
            {
                Config = ConfigurationParameterParser.ParseCommandLineArgs(args);
            }
            catch (InvalidCommandLineArgumentsException)
            {
                ShowUsage();
                return;
            }
            
            System.Console.WriteLine("Root Node GUID: " + Config.RootNodeId);

            List<SitecoreItem> sitecoreItems = TraverseSitecoreContentTree();

            System.Console.WriteLine(sitecoreItems.Count + " Sitecore URLs found in content tree" );

            if (Config.ListUrls)
            {
                ListSitecoreUrls(sitecoreItems);
                return;
            }

            if (Config.RunHttpChecker)
            {
                var httpRunner = new HttpPageCheckerRunner(Container);
                httpRunner.Run(Config, sitecoreItems);
            }
            if (Config.RunWebDriverChecker)
            {
                var webDriverRunner = new WebDriverPageCheckerRunner();
                webDriverRunner.Run(Config, sitecoreItems);
            }

            Environment.Exit(0);
        }

        private static void ListSitecoreUrls(List<SitecoreItem> sitecoreItems)
        {
            if (sitecoreItems.Count > 0)
            {
                System.Console.WriteLine("Url\tItem path");
            }

            foreach (SitecoreItem sitecoreItem in sitecoreItems)
            {                
                System.Console.WriteLine($"{sitecoreItem.ItemUrl}\t{sitecoreItem.ItemPath}");
            }
        }
        
        private static List<SitecoreItem> TraverseSitecoreContentTree()
        {
            IRestClient restClient = Container.Resolve<IRestClient>();
            ISitecoreServiceClient sitecoreServiceClient =  new SitecoreItemServiceClient(restClient, Config);

            try
            {
                List<SitecoreItem> sitecoreUrls = sitecoreServiceClient.GetSitecoreCmsTreeUrls();
                return sitecoreUrls;
            }
            catch (ServiceClientAuthorizationException)
            {
                System.Console.WriteLine("Unable to connect to Sitecore Services Client with the supplied credentials");
                Environment.Exit(1);
            }
            return null;
        }

        private static void RegisterIocModules()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterModule<PageCheckersModule>();
            builder.RegisterModule<ServicesClientModule>();
            Container = builder.Build();
        }

        private static void ShowUsage()
        {
            System.Console.WriteLine("Usage: sitecore-assurance -r {root node guid} -u {base url} [-l] [-h] [-s] ");
            System.Console.WriteLine("       sitecore-assurance --root {root node guid} --baseurl {base url} [--list] [--http] [--selenium]");
        }

    }
}
