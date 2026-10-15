using System.Collections.Concurrent;
using FrogGame.Application.Games;
using FrogGame.Domain.Games;

namespace FrogGame.Infrastructure.Games;

public sealed class InMemoryGameRepository : IGameRepository
{
    private readonly ConcurrentDictionary<Guid, GameSession> _games = new();

    public Task<GameSession?> FindAsync(Guid id, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        _games.TryGetValue(id, out var game);
        return Task.FromResult(game);
    }

    public Task SaveAsync(GameSession game, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        _games[game.Id] = game;
        return Task.CompletedTask;
    }
}
