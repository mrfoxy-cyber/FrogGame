namespace TheLab.Master.Contracts
{

  public class GetGameSettingsResponse
  {
    public bool Success { get; set; }
    public GameSetting[] Settings { get; set; }
  }
}
