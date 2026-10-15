using FrogGame.Domain.Scores;

namespace FrogGame.Domain.Games;

public sealed record ChainReaction(
    int Chain,
    Score Awarded,
    IReadOnlyList<BoardPosition> Matches,
    FrogColor?[][] BoardAfterGravity);

public sealed record MoveResolution(
    BoardPosition PlacedAt,
    FrogColor Frog,
    IReadOnlyList<ChainReaction> Reactions);

public sealed class GameSession
{
    private static readonly int[] LevelTargets = [100, 200, 360, 560];

    public GameSession(int rows, int columns, Frog firstFrog)
    {
        Id = Guid.NewGuid();
        Board = new GameBoard(rows, columns);
        NextFrog = firstFrog;
    }

    public Guid Id { get; }
    public GameBoard Board { get; }
    public Score Score { get; private set; } = Score.Zero;
    public Score BestCombo { get; private set; } = Score.Zero;
    public int Level { get; private set; }
    public Frog NextFrog { get; private set; }
    public GameStatus Status { get; private set; } = GameStatus.Playing;

    public MoveResolution Play(int column, IScoreCalculator scoreCalculator)
    {
        if (Status is not GameStatus.Playing)
        {
            throw new GameRuleException("This game has already ended.");
        }

        var playedFrog = NextFrog;
        var placedAt = Board.Drop(playedFrog, column);
        var reactions = new List<ChainReaction>();

        for (var chain = 1; ; chain++)
        {
            var matches = Board.FindMatches();
            if (matches.Count == 0)
            {
                break;
            }

            var awarded = scoreCalculator.Calculate(Board, matches, chain);
            Score = Score.Add(awarded.Value);
            if (awarded.Value > BestCombo.Value)
            {
                BestCombo = awarded;
            }

            Board.Remove(matches);
            Board.ApplyGravity();
            reactions.Add(new ChainReaction(chain, awarded, matches, Board.Snapshot()));
        }

        UpdateProgress();
        return new MoveResolution(placedAt, playedFrog.Color, reactions);
    }

    public void SetNextFrog(Frog frog)
    {
        if (Status is GameStatus.Playing)
        {
            NextFrog = frog;
        }
    }

    private void UpdateProgress()
    {
        if (Score.Value >= LevelTargets[Level])
        {
            if (Level < LevelTargets.Length - 1)
            {
                Level++;
            }
            else
            {
                Status = GameStatus.Completed;
            }
        }
        else if (Board.IsFull)
        {
            Status = GameStatus.BoardFull;
        }
    }
}
