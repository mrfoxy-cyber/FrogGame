using FrogGame.Application.Games;
using FrogGame.Domain.Games;

namespace FrogGame.Web.Endpoints;

public sealed record CreateGameRequest(int Rows, int Columns);
public sealed record PlayMoveRequest(int Column);

public static class GameEndpoints
{
    public static RouteGroupBuilder MapGameEndpoints(this RouteGroupBuilder api)
    {
        var games = api.MapGroup("/games");

        games.MapPost("/", CreateAsync);
        games.MapGet("/{id:guid}", GetAsync);
        games.MapPost("/{id:guid}/moves", PlayAsync);

        return api;
    }

    private static async Task<IResult> CreateAsync(
        CreateGameRequest request,
        GameService games,
        CancellationToken cancellationToken)
    {
        try
        {
            var game = await games.CreateAsync(
                new CreateGameCommand(request.Rows, request.Columns),
                cancellationToken);
            return Results.Created($"/api/games/{game.Id}", game);
        }
        catch (ArgumentOutOfRangeException exception)
        {
            return Validation(exception.ParamName ?? "board", exception.Message);
        }
    }

    private static async Task<IResult> GetAsync(
        Guid id,
        GameService games,
        CancellationToken cancellationToken)
    {
        try
        {
            return Results.Ok(await games.GetAsync(id, cancellationToken));
        }
        catch (GameNotFoundException exception)
        {
            return Results.NotFound(new { error = exception.Message });
        }
    }

    private static async Task<IResult> PlayAsync(
        Guid id,
        PlayMoveRequest request,
        GameService games,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await games.PlayAsync(
                new PlayMoveCommand(id, request.Column),
                cancellationToken);
            return Results.Ok(result);
        }
        catch (GameNotFoundException exception)
        {
            return Results.NotFound(new { error = exception.Message });
        }
        catch (GameRuleException exception)
        {
            return Results.Conflict(new { error = exception.Message });
        }
    }

    private static IResult Validation(string key, string message) =>
        Results.ValidationProblem(new Dictionary<string, string[]> { [key] = [message] });
}
