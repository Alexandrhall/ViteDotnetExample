using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;

namespace weather.Services
{
    public static class LocalDevelopment
    {
        public static void NpmRunDev(this WebApplication app)
        {
            app.MapWhen(context =>
            {
                var endpointDataSource = app.Services.GetRequiredService<EndpointDataSource>();

                // // Get all API routes
                var apiRoutes = endpointDataSource.Endpoints
                    .OfType<RouteEndpoint>()
                    .SelectMany(e => new[] { "/" + e.RoutePattern.RawText,
                     "/" + e.RoutePattern.RawText?.ToLowerInvariant() })
                    .ToList();
                // Add Swagger route
                apiRoutes.Add("/swagger");

                // Check if the request path is null
                if (context.Request.Path.Value == null) return false;

                // Exclude requests to Swagger and API routes
                var isApiRoute = apiRoutes.Any(route => context.Request.Path.Value.StartsWith(route));
                return !isApiRoute;

            }, spa =>
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