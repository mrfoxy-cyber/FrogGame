using System;

namespace TheLab.Master.Contracts
{
  public class RegisterGameScoreResultsRequest
  {
    public Guid InternalAuthToken { get; set; }
    public int GameId { get; set; }
    public PlayerGameScoreRegistration[] PlayerScoreRegistrations { get; set; }
  }
}
