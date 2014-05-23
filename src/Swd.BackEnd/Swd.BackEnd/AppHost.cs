using Funq;
using ServiceStack.ServiceInterface.Cors;
using ServiceStack.WebHost.Endpoints;
using Swd.BackEnd.Services;

namespace Swd.BackEnd
{
    public class AppHost : AppHostHttpListenerBase
    {
        public AppHost()
            : base("HttpListener Self-Host", typeof(UniversitiesService).Assembly) { }

        public override void Configure(Funq.Container container)
        {
            Plugins.Add(new CorsFeature());
        }
    }
}