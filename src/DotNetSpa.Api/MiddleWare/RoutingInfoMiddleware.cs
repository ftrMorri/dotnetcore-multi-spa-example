using Microsoft.AspNetCore.Http;
using System.Diagnostics;

namespace DotNetSpa.Api.MiddleWare
{
    public class RoutingInfoMiddleware
    {
        private readonly RequestDelegate _next;

        public RoutingInfoMiddleware(RequestDelegate requestDelegate)
        {
            _next = requestDelegate;
        }

        public Task Invoke(HttpContext httpContext)
        {
            var httpRequest = httpContext.Request;

            var nextTask = _next.Invoke(httpContext);
            nextTask.ContinueWith(t =>
            {
                if (t.Status == TaskStatus.RanToCompletion)
                {
                    // Console.WriteLine(httpContext.GetRouteData().Routers.Count());
                    Console.WriteLine(httpRequest.Path);
                }
                else
                {
                    Console.WriteLine(t.Exception);
                }
            });
            return nextTask;
        }
    }

    public static class RoutingInfoMiddlewareExtensions
    {
        public static IApplicationBuilder UseRoutingInfo(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RoutingInfoMiddleware>();
        }
    }
}
