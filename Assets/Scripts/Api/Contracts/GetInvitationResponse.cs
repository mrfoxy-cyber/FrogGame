namespace TheLab.Master.Contracts
{
  public class GetInvitationResponse
  {
    public bool Success { get; set; }
    public Invitation Invitation { get; set; }
    public GetInvitationResult Result { get; set; }
    public string Message { get; set; }
  }
}
