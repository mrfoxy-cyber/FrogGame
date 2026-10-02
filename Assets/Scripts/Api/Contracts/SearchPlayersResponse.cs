namespace TheLab.Master.Contracts
{
  public class SearchPlayersResponse : PagedResponse
  {
    public bool Success { get; set; }
    public int TotalMatchingPlayers { get; set; }
    public Player[] Players { get; set; }
  }
}
