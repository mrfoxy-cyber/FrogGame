using System.Collections.Concurrent;
using FrogGame.Application.Scores;
using FrogGame.Domain.Games;

namespace FrogGame.Application.Games;

public sealed class GameService(
    IGameRepository games,
    IFrogProvider frogs,
    IScoreCalculator scoreCalculator,
    ScoreService scores)
{
    private readonly ConcurrentDictionary<Guid, SemaphoreSlim> _gameLocks = new();

    public async Task<GameState> CreateAsync(CreateGameCommand command, CancellationToken cancellationToken)
    {
        var game = new GameSession(command.Rows, command.Columns, frogs.Next(level: 0));
        await games.SaveAsync(game, cancellationToken);
        return ToState(game);
    }

    public async Task<GameState> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        var gameLock = _gameLocks.GetOrAdd(id, _ => new SemaphoreSlim(1, 1));
        await gameLock.WaitAsync(cancellationToken);
        try
        {
            var game = await games.FindAsync(id, cancellationToken) ?? throw new GameNotFoundException(id);
            return ToState(game);
        }
        finally
        {
            gameLock.Release();
        }
    }

    public async Task<MoveResult> PlayAsync(PlayMoveCommand command, CancellationToken cancellationToken)
    {
        var gameLock = _gameLocks.GetOrAdd(command.GameId, _ => new SemaphoreSlim(1, 1));
        await gameLock.WaitAsync(cancellationToken);
        try
        {
            var game = await games.FindAsync(command.GameId, cancellationToken)
                ?? throw new GameNotFoundException(command.GameId);
            var resolution = game.Play(command.Column, scoreCalculator);
            game.SetNextFrog(frogs.Next(game.Level));
            await games.SaveAsync(game, cancellationToken);
            if (game.Status is not GameStatus.Playing && game.Score.Value > 0)
            {
                await scores.SubmitAsync(
                    new SubmitScoreCommand(game.Score.Value, game.BestCombo.Value),
                    cancellationToken);
            }

            return new MoveResult(
                resolution.PlacedAt,
                resolution.Frog,
                resolution.Reactions.Select(reaction => new ChainReactionResult(
                    reaction.Chain,
                    reaction.Awarded.Value,
                    reaction.Matches,
                    reaction.BoardAfterGravity)).ToArray(),
                ToState(game));
        }
        finally
        {
            gameLock.Release();
        }
    }

    private static GameState ToState(GameSession game) => new(
        game.Id,
        game.Board.Rows,
        game.Board.Columns,
        game.Board.Snapshot(),
        game.Score.Value,
        game.BestCombo.Value,
        game.Level,
        game.NextFrog.Color,
        game.Status);
}
