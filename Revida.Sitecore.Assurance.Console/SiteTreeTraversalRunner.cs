using System;
using System.Collections.Generic;
using System.Net;
using Autofac;
using RestSharp;
using Revida.Sitecore.Assurance.Configuration;
using Revida.Sitecore.Assurance.Model;
using Revida.Sitecore.Services.Client;

namespace Revida.Sitecore.Assurance.Console
{
    public class SiteTreeTraversalRunner : RunnerBase
    {
        private IContainer Container { get; }

        private ConfigurationParameters Config { get;  }

        public SiteTreeTraversalRunner(IContainer container, ConfigurationParameters config)
        {
            Container = container;
            Config = config;
        }

        public List<SitecoreItem> Run()
        {
            IRestClient restClient = Container.Resolve<IRestClient>();
            restClient.CookieContainer = new CookieContainer();
            ISitecoreServiceClient sitecoreServiceClient = new SitecoreItemServiceClient(restClient, Config);

            try
            {
                List<SitecoreItem> sitecoreUrls = sitecoreServiceClient.GetSitecoreCmsTreeUrls();
                return sitecoreUrls;
            }
            catch (ServiceClientAuthorizationException)
            {
                System.Console.WriteLine("Unable to connect to Sitecore Services Client with the supplied credentials or anonymous access is not enabled");
                Environment.Exit(1);
            }
            return null;
        }
    }
}
