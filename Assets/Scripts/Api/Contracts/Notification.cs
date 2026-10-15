namespace TheLab.Master.Contracts
{

  public class Notification
  {
    public int Id { get; set; }
    public NotificationType NotificationType { get; set; }
    public string Data { get; set; }
    public bool HasBeenViewed { get; set; }
    public int ReferenceId { get; set; }
  }
}
