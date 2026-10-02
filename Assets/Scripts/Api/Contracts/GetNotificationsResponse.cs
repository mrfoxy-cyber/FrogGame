namespace TheLab.Master.Contracts
{

  public class GetNotificationsResponse : PagedResponse
  {
    public bool Success { get; set; }
    public Notification[] Notifications { get; set; }
  }
}
