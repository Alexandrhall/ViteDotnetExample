using Microsoft.OpenApi;
using weather.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddHealthChecks();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "WeatherAPI", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline
// Swagger runs before the SPA handling so /swagger never reaches the frontend.
// Toggle per environment with the EnableSwagger setting (env var: EnableSwagger=false)
if (app.Configuration.GetValue("EnableSwagger", true))
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "WeatherAPI V1");
    });
}

if (!app.Environment.IsDevelopment())
{
    // In production, serve the built React app from wwwroot
    app.UseStaticFiles();
}

app.UseRouting();
app.MapControllers();
app.MapHealthChecks("/health");

if (!app.Environment.IsDevelopment())
{
    // Every path that doesn't match a controller is handled by React
    app.MapFallbackToFile("index.html");
}
else
{
    // In development, proxy every path that doesn't match a controller to the Vite dev server
    app.NpmRunDev();
}

app.Run();
