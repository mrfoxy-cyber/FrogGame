namespace TheLab.Master.Contracts
{

  public class SendInviteResponse
  {
    public bool Success { get; set; }
    public string Message { get; set; }
    public SendInviteResult Result { get; set; }
  }
}
