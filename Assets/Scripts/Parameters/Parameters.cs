using UnityEngine;

public class Parameters : MonoBehaviour
{
  public enum LevelType
  {
    MaxPoints = 0,
    MaxCombo = 1,
    Battle = 2
  };
  public enum FrogTypes
  {
    Red = 1,
    Yellow = 2,
    Green = 3,
    Blue = 4,
    White = 5,
    Black = 6,
    Grey = 7,
    Pink = 8,
    Purple = 9,
    Orange = 10,
    Empty = -1
  };

  public enum FrogStatus
  {
    inCombo = 0,
    falling = 1,
    idle = 2,
    moving = 3,
    intoBlob = 4,
    connected = 5,
    beingeaten = 6,

  };

  //defines where it is combined with froggies Right, Left, Up, Down
  public enum FroggyPlacement
  {
    S = 0,
    R = 1,
    L = 2,
    U = 3,
    D = 4,
    RL = 5,
    UL = 6,
    DL = 7,
    UD = 8,
    UR = 9,
    DR = 10,
    RUL = 11,
    RLD = 12,
    LUD = 13,
    RUD = 14,
    LURD = 15,
  };
}
