namespace TheLab.Master.Contracts
{
  public class GetGameScoreResultsReponse
  {
    public bool Success { get; set; }
    public GetGameScoreResultsResult Result { get; set; }
    public string Message { get; set; }
    public int GameId { get; set; }
    public string GameMode { get; set; }
    public PlayerGameScore[] PlayerScores { get; set; }
  }
}
