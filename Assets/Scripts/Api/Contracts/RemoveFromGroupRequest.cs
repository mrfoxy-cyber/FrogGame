namespace TheLab.Master.Contracts
{
  using System;

  public class RemoveFromGroupRequest
  {
    public Guid Token { get; set; }
    public int RemovePlayerId { get; set; }
  }
}
