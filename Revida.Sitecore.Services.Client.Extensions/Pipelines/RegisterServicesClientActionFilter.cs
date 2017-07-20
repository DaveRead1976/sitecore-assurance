using System.Diagnostics.CodeAnalysis;
using System.Web.Http;
using Revida.Sitecore.Services.Client.Extensions.Filters;
using Sitecore.Pipelines;

namespace Revida.Sitecore.Services.Client.Extensions.Pipelines
{
    [ExcludeFromCodeCoverage]
    public class RegisterServicesClientActionFilter
    {
        public void Process(PipelineArgs args)
        {
            GlobalConfiguration.Configuration.Filters.Add(new ItemModelActionFilter());
        }
    }
}
