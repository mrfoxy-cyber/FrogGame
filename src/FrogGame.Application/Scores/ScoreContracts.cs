using FrogGame.Domain.Scores;

namespace FrogGame.Application.Scores;

public sealed record SubmitScoreCommand(int Score, int BestCombo);

public interface IScoreRepository
{
    Task<ScoreBoard> GetAsync(CancellationToken cancellationToken);
    Task<ScoreBoard> AddAsync(Score score, Score bestCombo, CancellationToken cancellationToken);
}
