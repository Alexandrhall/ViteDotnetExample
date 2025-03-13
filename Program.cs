using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.OpenApi.Models;
using weather.Services;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddOpenApi();
        builder.Services.AddControllers();
        builder.Services.AddControllersWithViews();
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAllOrigins", policy =>
            {
                policy.AllowAnyOrigin()  // Tillåter alla ursprung
                      .AllowAnyMethod()  // Tillåter alla HTTP-metoder (GET, POST, etc.)
                      .AllowAnyHeader(); // Tillåter alla headers
            });
        });

        builder.Services.AddControllers();
        builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WeatherAPI", Version = "v1" });
            });

        var port = Environment.GetEnvironmentVariable("ASPNETCORE_URLS") ?? "http://+:5057";
        builder.WebHost.UseUrls(port);

        var app = builder.Build();

        app.UseCors("AllowAllOrigins");
        app.MapControllers();

        if (!app.Environment.IsDevelopment())
        {
            // I produktion använd statiska filer från wwwroot
            app.UseStaticFiles();  // Serva React från wwwroot när den är byggd
            app.MapFallbackToFile("index.html");
            app.UseHttpsRedirection();
        }
        else
        {
            LocalNpm.NpmRunDev(app);
        }

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "WeatherAPI V1");
        });


        app.Run();
    }
}