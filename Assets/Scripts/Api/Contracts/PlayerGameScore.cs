using System.Collections.Generic;

namespace TheLab.Master.Contracts
{
  public class PlayerGameScore
  {
    public int PlayerId { get; set; }
    public string PlayerName { get; set; }
    public int Score { get; set; }
    public int Deaths { get; set; }
    public Dictionary<string, string> PlayerInformation { get; set; }
  }
}
