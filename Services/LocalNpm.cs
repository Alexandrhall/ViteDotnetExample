using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;

namespace weather.Services
{
    public static class LocalNpm
    {
        public static void NpmRunDev(WebApplication app)
        {
            app.MapWhen(context =>
            {
                var endpointDataSource = app.Services.GetRequiredService<EndpointDataSource>();

                // Hämta alla API-routes dynamiskt vid varje request
                var apiRoutes = endpointDataSource.Endpoints
                    .OfType<RouteEndpoint>()
                    .Select(e => "/" + e.RoutePattern.RawText)
                    .Union(endpointDataSource.Endpoints
                        .OfType<RouteEndpoint>()
                        .Select(e => "/" + e.RoutePattern.RawText?.ToLowerInvariant())
                    )
                    .ToList();

                if (context.Request.Path.Value == null) return false;

                return !context.Request.Path.StartsWithSegments("/swagger") &&
                       !apiRoutes.Any(route => context.Request.Path.Value.StartsWith(route));
            }
                        , spa =>
                        {
                            spa.UseSpa(spaBuilder =>
                            {
                                spaBuilder.Options.SourcePath = "clientapp";
                                spaBuilder.Options.DevServerPort = 5173;
                                spaBuilder.Options.DefaultPage = "/";
                                spaBuilder.UseReactDevelopmentServer(npmScript: "start");
                            });
                        });
        }
    }
}