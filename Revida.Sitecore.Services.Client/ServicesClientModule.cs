using System.Diagnostics.CodeAnalysis;
using Autofac;
using RestSharp;

namespace Revida.Sitecore.Services.Client
{   
    [ExcludeFromCodeCoverage]
    public class ServicesClientModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<RestClient>().As<IRestClient>();
        }
    }
}
