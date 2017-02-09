
namespace Revida.Sitecore.Services.Client
{
    public class SitecoreClientFactory
    {
        public ISitecoreServiceClient GetServiceClient()
        {
            return new SitecoreItemServiceClient();
        }
    }
}
