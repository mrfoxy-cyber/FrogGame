using UnityEngine;
using static Parameters;

[CreateAssetMenu(fileName = "New Level", menuName = "level")]
public class Levels : ScriptableObject
{
  public int numberOfFrogs;
  public int timeLimit;
  public int goalPoints;
  public Sprite background;
  public LevelType leveltype;
  public bool locked;
  public float cloudSpeed;
}
