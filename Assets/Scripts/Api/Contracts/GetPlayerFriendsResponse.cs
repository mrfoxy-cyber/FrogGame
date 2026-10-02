namespace TheLab.Master.Contracts
{
  public class GetPlayerFriendsResponse : PagedResponse
  {
    public bool Success { get; set; }
    public string Message { get; set; }
    public int TotalFriendsCount { get; set; }
    public Player[] Players { get; set; }
  }
}
