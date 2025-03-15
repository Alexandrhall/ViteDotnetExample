using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;

namespace weather.Services
{
    public static class LocalDevelopment
    {
        public static void NpmRunDev(WebApplication app)
        {
            app.MapWhen(context =>
            {
                var endpointDataSource = app.Services.GetRequiredService<EndpointDataSource>();

                // Dynamically fetch all API routes for each request
                var apiRoutes = endpointDataSource.Endpoints
                    .OfType<RouteEndpoint>()
                    .Select(e => "/" + e.RoutePattern.RawText)
                    .Union(endpointDataSource.Endpoints
                        .OfType<RouteEndpoint>()
                        .Select(e => "/" + e.RoutePattern.RawText?.ToLowerInvariant())
                    )
                    .ToList();

                // Check if the request path is null
                if (context.Request.Path.Value == null) return false;

                // Exclude requests to Swagger and API routes
                return !context.Request.Path.StartsWithSegments("/swagger") &&
                       !apiRoutes.Any(route => context.Request.Path.Value.StartsWith(route));
            }
            , spa =>
            {
                spa.UseSpa(spaBuilder =>
                {
                    // Set the source path for the SPA
                    spaBuilder.Options.SourcePath = "clientapp";
                    spaBuilder.Options.DevServerPort = 5173;

                    // Use React development server with the specified npm script
                    spaBuilder.UseReactDevelopmentServer(npmScript: "start");
                });
            });
        }
    }
}