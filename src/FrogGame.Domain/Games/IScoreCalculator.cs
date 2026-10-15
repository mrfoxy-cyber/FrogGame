using FrogGame.Domain.Scores;

namespace FrogGame.Domain.Games;

public interface IScoreCalculator
{
    Score Calculate(GameBoard board, IReadOnlyCollection<BoardPosition> matches, int chain);
}

public sealed class ClassicScoreCalculator : IScoreCalculator
{
    public Score Calculate(GameBoard board, IReadOnlyCollection<BoardPosition> matches, int chain)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(chain, 1);
        var frogPoints = matches.Sum(position => board[position]?.Points ?? 0);
        return new Score(checked(frogPoints * chain));
    }
}
