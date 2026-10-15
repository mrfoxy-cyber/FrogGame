namespace FrogGame.Domain.Games;

public sealed class GameRuleException(string message) : InvalidOperationException(message);
