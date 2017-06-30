using System;
using System.Collections.Generic;
using Autofac;
using RestSharp;
using Revida.Sitecore.Assurance.Configuration;
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

            PerformChecksOnSitecoreItems(sitecoreItems);
        }

        private static void PerformChecksOnSitecoreItems(List<SitecoreItem> sitecoreItems)
        {
            if (sitecoreItems.Count > 0)
            {
                System.Console.WriteLine("Success?\tStatus Code\tItem path");
            }

            PageHttpResponseChecker checker = Container.Resolve<PageHttpResponseChecker>();

            foreach (SitecoreItem sitecoreItem in sitecoreItems)
            {
                Uri pageUrl = new Uri($"{Config.BaseUrl}/{sitecoreItem.ItemUrl}");

                PageCheckResult result = checker.PageResponseValid(pageUrl);

                System.Console.WriteLine($"{result.Success}\t{result.StatusCode}\t{sitecoreItem.ItemPath}");
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
            System.Console.WriteLine("Usage: sitecore-assurance -r {root node guid} -u {base url}");
            System.Console.WriteLine("       sitecore-assurance --root {root node guid} --baseurl {base url}");
        }

    }
}
