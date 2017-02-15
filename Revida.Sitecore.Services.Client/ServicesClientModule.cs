using Autofac;
using RestSharp;

namespace Revida.Sitecore.Services.Client
{
    public class ServicesClientModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<RestClient>().As<IRestClient>();
        }
    }
}
