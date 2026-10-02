namespace Assets.TheLab.Scripts.MessageBus.Messages
{
  public class PlayerKilledMessage
  {
    public string ByEnemyName { get; set; }
    public bool IsBot { get; set; }
    public bool IsLocal { get; set; }
  }
}
