using Funq;
using ServiceStack;
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

            this.RequestFilters.Add((httpReq, httpRes, requestDto) =>
            {
                //Handles Request and closes Responses after emitting global HTTP Headers
                if (httpReq.HttpMethod == "OPTIONS")
                    httpRes.EndRequest();
            });
        }
    }
}