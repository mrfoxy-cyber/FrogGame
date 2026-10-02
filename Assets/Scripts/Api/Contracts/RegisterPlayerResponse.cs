namespace TheLab.Master.Contracts
{
  public class RegisterPlayerResponse
  {
    public bool Success { get; set; }
    public string Token { get; set; }
    public string Message { get; set; }
    public PlayerNameResult Result { get; set; }
  }
}
