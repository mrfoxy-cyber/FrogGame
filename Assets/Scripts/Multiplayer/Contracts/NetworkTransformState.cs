using RiptideNetworking;

public class NetworkTransformState
{
  public bool IsLocal;
  public ushort OwnerClientId;
  public ushort NetworkTransformId;
  public float PositionX, PositionY, PositionZ;
  public float EulerAnglesX, EulerAnglesY, EulerAnglesZ;

  public Message AppendToMessage(Message msg)
  {
    msg.AddUShort(OwnerClientId);
    msg.AddUShort(NetworkTransformId);
    msg.AddFloat(PositionX);
    msg.AddFloat(PositionY);
    msg.AddFloat(PositionZ);
    msg.AddFloat(EulerAnglesX);
    msg.AddFloat(EulerAnglesY);
    msg.AddFloat(EulerAnglesZ);
    return msg;
  }

  public static NetworkTransformState FromMessage(Message msg)
  {
    var networkTransformState = new NetworkTransformState();
    networkTransformState.OwnerClientId = msg.GetUShort();
    networkTransformState.NetworkTransformId = msg.GetUShort();
    networkTransformState.PositionX = msg.GetFloat();
    networkTransformState.PositionY = msg.GetFloat();
    networkTransformState.PositionZ = msg.GetFloat();
    networkTransformState.EulerAnglesX = msg.GetFloat();
    networkTransformState.EulerAnglesY = msg.GetFloat();
    networkTransformState.EulerAnglesZ = msg.GetFloat();
    return networkTransformState;
  }
}