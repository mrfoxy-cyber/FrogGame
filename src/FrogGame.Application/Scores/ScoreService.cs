using FrogGame.Domain.Scores;

namespace FrogGame.Application.Scores;

public sealed class ScoreService(IScoreRepository scores)
{
    public Task<ScoreBoard> GetAsync(CancellationToken cancellationToken) =>
        scores.GetAsync(cancellationToken);

    public Task<ScoreBoard> SubmitAsync(SubmitScoreCommand command, CancellationToken cancellationToken)
    {
        if (command.Score is < 1 or > 100_000_000)
        {
            throw new ArgumentOutOfRangeException(nameof(command.Score), "Score must be between 1 and 100,000,000.");
        }

        if (command.BestCombo is < 0 or > 100_000_000)
        {
            throw new ArgumentOutOfRangeException(nameof(command.BestCombo), "Best combo must be between 0 and 100,000,000.");
        }

        return scores.AddAsync(new Score(command.Score), new Score(command.BestCombo), cancellationToken);
    }
}
