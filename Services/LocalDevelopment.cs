using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;

namespace weather.Services
{
    public static class LocalDevelopment
    {
        public static void NpmRunDev(this WebApplication app)
        {
            // UseRouting has already run, so a request without an endpoint
            // didn't match any controller and belongs to the frontend
            app.MapWhen(context => context.GetEndpoint() is null, spa =>
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