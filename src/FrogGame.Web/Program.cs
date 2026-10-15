using System.Text.Json.Serialization;
using FrogGame.Application.Games;
using FrogGame.Application.Scores;
using FrogGame.Domain.Games;
using FrogGame.Infrastructure.Games;
using FrogGame.Infrastructure.Scores;
using FrogGame.Web.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddProblemDetails();

builder.Services.AddSingleton<IScoreCalculator, ClassicScoreCalculator>();
builder.Services.AddSingleton<IGameRepository, InMemoryGameRepository>();
builder.Services.AddSingleton<IFrogProvider, RandomFrogProvider>();
builder.Services.AddSingleton<IScoreRepository>(_ =>
    new JsonScoreRepository(Path.Combine(builder.Environment.ContentRootPath, "App_Data")));
builder.Services.AddSingleton<GameService>();
builder.Services.AddSingleton<ScoreService>();

var app = builder.Build();

app.UseExceptionHandler();
app.UseDefaultFiles();
app.UseStaticFiles();

var api = app.MapGroup("/api");
api.MapGet("/health", () => Results.Ok(new
{
    status = "ok",
    game = "Froggy Puzzle Pop",
    utcTime = DateTimeOffset.UtcNow
}));
api.MapGameEndpoints();
api.MapScoreEndpoints();

app.Map("/api/{**path}", () => Results.NotFound());
app.MapFallbackToFile("index.html");

app.Run();
