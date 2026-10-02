namespace TheLab.Master.Contracts
{
  using System;

  public class GetInvitationRequest
  {
    public Guid Token { get; set; }
    public int InvitationId { get; set; }
  }
}
