namespace TheLab.Master.Contracts
{
  using System;

  public class GetInvitationsRequest
  {
    public Guid Token { get; set; }
    public int Page { get; set; }
    public InvitationType? FilterByType { get; set; } = null;
  }
}
