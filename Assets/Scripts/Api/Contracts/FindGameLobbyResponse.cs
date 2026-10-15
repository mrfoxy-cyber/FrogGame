namespace TheLab.Master.Contracts
{
  public class FindGameLobbyResponse
  {
    public bool Success { get; set; }
    public string GameMode { get; set; }
    public int Port { get; set; }
    public string Message { get; set; }
    public FindGameLobbyResult Result { get; set; }
  }
}
