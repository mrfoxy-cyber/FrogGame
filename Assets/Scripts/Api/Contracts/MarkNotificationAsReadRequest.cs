namespace TheLab.Master.Contracts
{
  public class MarkNotificationAsReadRequest
  {
    public string Token { get; set; }
    public int NotificationId { get; set; }
  }
}
