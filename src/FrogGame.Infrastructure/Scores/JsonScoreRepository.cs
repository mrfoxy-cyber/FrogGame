using System.Text.Json;
using FrogGame.Application.Scores;
using FrogGame.Domain.Scores;

namespace FrogGame.Infrastructure.Scores;

public sealed class JsonScoreRepository(string dataDirectory) : IScoreRepository
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        WriteIndented = true
    };

    private readonly SemaphoreSlim _gate = new(1, 1);
    private readonly string _file = Path.Combine(dataDirectory, "scores.json");

    public async Task<ScoreBoard> GetAsync(CancellationToken cancellationToken)
    {
        await _gate.WaitAsync(cancellationToken);
        try
        {
            return await ReadAsync(cancellationToken);
        }
        finally
        {
            _gate.Release();
        }
    }

    public async Task<ScoreBoard> AddAsync(
        Score score,
        Score bestCombo,
        CancellationToken cancellationToken)
    {
        await _gate.WaitAsync(cancellationToken);
        try
        {
            var current = await ReadAsync(cancellationToken);
            var entries = current.Scores
                .Append(new ScoreEntry(score.Value, DateTimeOffset.UtcNow))
                .OrderByDescending(entry => entry.Score)
                .ThenBy(entry => entry.PlayedAt)
                .Take(10)
                .ToArray();
            var updated = new ScoreBoard(
                Math.Max(current.BestScore, score.Value),
                Math.Max(current.BestCombo, bestCombo.Value),
                entries);

            Directory.CreateDirectory(dataDirectory);
            var temporaryFile = _file + ".tmp";
            await using (var stream = File.Create(temporaryFile))
            {
                await JsonSerializer.SerializeAsync(stream, updated, JsonOptions, cancellationToken);
            }

            File.Move(temporaryFile, _file, true);
            return updated;
        }
        finally
        {
            _gate.Release();
        }
    }

    private async Task<ScoreBoard> ReadAsync(CancellationToken cancellationToken)
    {
        if (!File.Exists(_file))
        {
            return ScoreBoard.Empty;
        }

        try
        {
            await using var stream = File.OpenRead(_file);
            return await JsonSerializer.DeserializeAsync<ScoreBoard>(stream, JsonOptions, cancellationToken)
                ?? ScoreBoard.Empty;
        }
        catch (JsonException)
        {
            return ScoreBoard.Empty;
        }
    }
}
