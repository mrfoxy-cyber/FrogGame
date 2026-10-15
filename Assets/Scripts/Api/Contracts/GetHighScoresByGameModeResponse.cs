public class GetHighScoresByGameModeResponse
{
  public HighScoreEntry[] HighScores { get; set; }
  public GetHighScoresByGameModeResult Result { get; set; }
  public bool Success { get; set; }
}
