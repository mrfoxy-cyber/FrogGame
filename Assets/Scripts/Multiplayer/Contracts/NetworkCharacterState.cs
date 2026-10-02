using RiptideNetworking;

public class NetworkCharacterState
{
  public int PlayerId;
  public ushort ClientId;
  public ushort CharacterId;
  public ushort Score;
  public bool IsLocal;
  public bool ClimbingLadder;
  public float MovementAxisX, MovementAxisY;
  public float PositionX, PositionY, PositionZ;
  public float EulerAnglesX, EulerAnglesY, EulerAnglesZ;
  public byte CurrentWeaponType;
  public byte Health;
  public bool IsGrounded;
  public bool IsPinned;

  public Message AppendToMessage(Message msg)
  {
    msg.AddInt(PlayerId);
    msg.AddUShort(ClientId);
    msg.AddUShort(CharacterId);
    msg.AddUShort(Score);

    msg.AddBool(ClimbingLadder);
    if (!ClimbingLadder)
    {
      msg.AddFloat(MovementAxisX);
      msg.AddFloat(MovementAxisY);
    }

    msg.AddFloat(PositionX);
    msg.AddFloat(PositionY);
    msg.AddFloat(PositionZ);
    msg.AddFloat(EulerAnglesX);
    msg.AddFloat(EulerAnglesY);
    msg.AddFloat(EulerAnglesZ);
    msg.AddByte(CurrentWeaponType);
    msg.AddByte(Health);
    msg.AddBool(IsGrounded);
    msg.AddBool(IsPinned);
    return msg;
  }

  public static NetworkCharacterState FromMessage(Message msg)
  {
    var characterState = new NetworkCharacterState();
    characterState.PlayerId = msg.GetInt();
    characterState.ClientId = msg.GetUShort();
    characterState.CharacterId = msg.GetUShort();
    characterState.Score = msg.GetUShort();

    characterState.ClimbingLadder = msg.GetBool();
    if (!characterState.ClimbingLadder)
    {
      characterState.MovementAxisX = msg.GetFloat();
      characterState.MovementAxisY = msg.GetFloat();
    }

    characterState.PositionX = msg.GetFloat();
    characterState.PositionY = msg.GetFloat();
    characterState.PositionZ = msg.GetFloat();
    characterState.EulerAnglesX = msg.GetFloat();
    characterState.EulerAnglesY = msg.GetFloat();
    characterState.EulerAnglesZ = msg.GetFloat();
    characterState.CurrentWeaponType = msg.GetByte();
    characterState.Health = msg.GetByte();
    characterState.IsGrounded = msg.GetBool();
    characterState.IsPinned = msg.GetBool();
    return characterState;
  }
}