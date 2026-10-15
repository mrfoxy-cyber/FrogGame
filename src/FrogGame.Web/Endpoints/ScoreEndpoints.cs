using FrogGame.Application.Scores;

namespace FrogGame.Web.Endpoints;

public static class ScoreEndpoints
{
    public static RouteGroupBuilder MapScoreEndpoints(this RouteGroupBuilder api)
    {
        api.MapGet("/scores", async (ScoreService scores, CancellationToken cancellationToken) =>
            Results.Ok(await scores.GetAsync(cancellationToken)));

        return api;
    }
}
