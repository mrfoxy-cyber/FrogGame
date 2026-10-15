namespace Assets.TheLab.Scripts.MessageBus.Messages
{
  public class KillStreakUpdateMessage
  {
    public ushort NetworkCharacterId { get; set; }
    public bool IsBot { get; set; }
    public int KillStreak { get; set; }
    public int TotalKills { get; set; }
  }
}
