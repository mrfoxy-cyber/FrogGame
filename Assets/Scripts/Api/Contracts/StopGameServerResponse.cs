namespace TheLab.Master.Contracts
{
  public class StopGameServerResponse
  {
    public bool Success { get; set; }
    public StopGameServerResult Result { get; set; }
    public string Message { get; set; }
  }
}
