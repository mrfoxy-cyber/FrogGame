namespace TheLab.Master.Contracts
{
  public class SetPLayerInformationResponse
  {
    public bool Success { get; set; }
    public string Message { get; set; }
    public SetPlayerInformationResult Result { get; set; }
  }
}
