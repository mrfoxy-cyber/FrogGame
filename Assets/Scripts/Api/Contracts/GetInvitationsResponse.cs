namespace TheLab.Master.Contracts
{
  public class GetInvitationsResponse
  {
    public bool Success { get; set; }
    public Invitation[] Invitations { get; set; }
    public GetInvitationsResult Result { get; set; }
    public string Message { get; set; }
  }
}
