using UnityEngine;

public class PlayerRespawnedMessage
{
  public string BobId { get; set; }
  public int SpawnPointId { get; set; }
  public bool IsLocal { get; set; }
  public bool IsBot { get; set; }
  public Transform FollowTransform { get; set; }
}
