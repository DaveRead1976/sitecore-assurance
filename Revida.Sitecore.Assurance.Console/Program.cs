using System.CodeDom;
using Autofac;
using Revida.Sitecore.Assurance.Configuration;
using Revida.Sitecore.Assurance.PageCheckers;

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
                System.Console.WriteLine("Usage: sitecore-assurance -r {root node guid} [-s {service version}]");
                return;
            }

            System.Console.WriteLine("Sitecore Client Version: " + Config.SiteCoreClient);
            System.Console.WriteLine("Root Node GUID: " + Config.RootNodeId);
        }

        private static void RegisterIocModules()
        {
            ContainerBuilder builder = new ContainerBuilder();
            Container = builder.Build();
            builder.RegisterModule<PageCheckersModule>();
        }
    }
}
