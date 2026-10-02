namespace TheLab.Master.Contracts
{
  public class SetPlayerNameResponse
  {
    public bool Success { get; set; }
    public PlayerNameResult Result { get; set; }
    public string Message { get; set; }
  }
}
