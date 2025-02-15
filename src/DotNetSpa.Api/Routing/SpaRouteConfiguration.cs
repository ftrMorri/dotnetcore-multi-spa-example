namespace DotNetSpa.Api.Routing
{
    public class SpaRouteConfiguration
    {
        public SpaRouteConfiguration(string routePath, string rootPath, string developmentServerUrl)
        {
            RoutePath = routePath;
            RootPath = rootPath;
            DevelopmentServerUrl = developmentServerUrl;
        }

        public string RoutePath { get; set; } = "/";
        public string RootPath { get; set; } = "wwwroot";
        public string DevelopmentServerUrl { get; set; } = "http://localhost:1234";
    }
}
