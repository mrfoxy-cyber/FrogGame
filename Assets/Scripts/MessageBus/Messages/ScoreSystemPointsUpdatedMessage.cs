public class ScoreSystemPointsUpdatedMessage
{
  public bool IsBot { get; set; }
  public ushort NetworkCharacterId { get; set; }
  public int ScoreSystemTypeInt { get; set; }
  public float Points { get; set; }
  public int MaxSurvivorsCount { get; set; }
  public int CurrentNumberOfSurvivors { get; set; }
}
