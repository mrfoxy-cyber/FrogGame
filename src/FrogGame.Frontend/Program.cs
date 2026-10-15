using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/config.js", (IConfiguration configuration) =>
{
    var apiBaseUrl = configuration["ApiBaseUrl"]?.TrimEnd('/') ?? "http://localhost:64909";
    var javascript = $"window.FROG_API_BASE = {JsonSerializer.Serialize(apiBaseUrl)};";
    return Results.Text(javascript, "application/javascript");
});

app.UseDefaultFiles();
app.UseStaticFiles();
app.MapFallbackToFile("index.html");

app.Run();
