namespace FrogGame.Application.Games;

public sealed class GameNotFoundException(Guid id)
    : KeyNotFoundException($"Game '{id}' was not found.");
