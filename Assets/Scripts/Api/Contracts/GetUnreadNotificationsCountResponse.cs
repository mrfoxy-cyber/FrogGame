namespace TheLab.Master.Contracts
{
  public class GetUnreadNotificationsCountResponse
  {
    public bool Success { get; set; }
    public string Message { get; set; }
    public int Count { get; set; }
    public GetUnreadNotificationsCountResult Result { get; set; }
  }
}
