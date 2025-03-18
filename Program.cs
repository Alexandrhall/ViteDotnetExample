using Microsoft.OpenApi.Models;
using weather.Services;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;

// Add services to the container
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddControllersWithViews();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.AllowAnyOrigin()  // Allow all origins
              .AllowAnyMethod()  // Allow all HTTP methods (GET, POST, etc.)
              .AllowAnyHeader(); // Allow all headers
    });
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "WeatherAPI", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline
app.UseCors("AllowAllOrigins");
app.MapControllers();

if (!app.Environment.IsDevelopment())
{
    // In production, serve static files from wwwroot
    app.UseStaticFiles();  // Serve React from wwwroot when built
    app.MapFallbackToFile("index.html");
    app.UseHttpsRedirection();
}
else
{
    // In development, use the React development server
    app.NpmRunDev();
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "WeatherAPI V1");
});

app.Run();
