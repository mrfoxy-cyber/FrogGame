namespace TheLab.Master.Contracts
{

  public class RemoveFromGroupResponse
  {
    public bool Success { get; set; }
    public string Message { get; set; }
    public RemoveFromGroupResult Result { get; set; }
  }
}
