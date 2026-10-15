using FrogGame.Domain.Games;

namespace FrogGame.Application.Games;

public interface IGameRepository
{
    Task<GameSession?> FindAsync(Guid id, CancellationToken cancellationToken);
    Task SaveAsync(GameSession game, CancellationToken cancellationToken);
}

public interface IFrogProvider
{
    Frog Next(int level);
}
