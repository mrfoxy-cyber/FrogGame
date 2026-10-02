using System.Collections.Generic;

namespace TheLab.Master.Contracts
{
  public class Player
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public Dictionary<string, string> PlayerInformation { get; set; }
  }
}
