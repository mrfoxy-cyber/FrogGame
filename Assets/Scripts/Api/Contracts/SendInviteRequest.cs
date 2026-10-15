namespace TheLab.Master.Contracts
{
  public class SendInviteRequest
  {
    public string Token { get; set; }
    public int InvitePlayerId { get; set; }
    public InvitationType Type { get; set; }
  }
}
