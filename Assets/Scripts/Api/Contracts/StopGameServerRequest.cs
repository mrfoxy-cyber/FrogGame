namespace TheLab.Master.Contracts
{
  using System;

  public class StopGameServerRequest
  {
    public Guid InternalAuthToken { get; set; }
    public int Port { get; set; }
  }
}
