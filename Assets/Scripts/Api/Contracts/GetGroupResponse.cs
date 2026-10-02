namespace TheLab.Master.Contracts
{
  public class GetGroupResponse
  {
    public bool Success { get; set; }
    public GetGroupResult Result { get; set; }
    public Player[] PlayersInGroup { get; set; }
    public GroupActivityStatus Status { get; set; }
    public int ActiveGamePort { get; set; }
    public string ActiveGameMode { get; set; }
  }
}
