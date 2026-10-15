namespace FrogGame.Domain.Scores;

public sealed record ScoreEntry(int Score, DateTimeOffset PlayedAt);

public sealed record ScoreBoard(int BestScore, int BestCombo, IReadOnlyList<ScoreEntry> Scores)
{
    public static ScoreBoard Empty { get; } = new(0, 0, []);
}
