namespace TheLab.Master.Contracts
{
  public class RegisterGameScoreResponse
  {
    public bool Success { get; set; }
    public RegisterGameScoreResultsResult Result { get; set; }
    public string Messsage { get; set; }
  }
}
