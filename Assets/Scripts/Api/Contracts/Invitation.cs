namespace TheLab.Master.Contracts
{
  public class Invitation
  {
    public int InvitationId { get; set; }
    public InvitationType Type { get; set; }
    public string Text { get; set; }
  }
}
