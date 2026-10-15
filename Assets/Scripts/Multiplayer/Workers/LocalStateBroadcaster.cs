using RiptideNetworking;

public static class LocalStateBroadcaster
{
  public static void BroadcastLocalState(Client client)
  {
    var localCharacterState = CharacterRepository.GetLocalMultiplayerCharacterStates();
    var localNetworkTransforms = NetworkTransformRepository.GetLocalNetworkTransformStates();
    var message = Message.Create(MessageSendMode.unreliable, (ushort)GameServerMessageType.GameState);
    message.AddByte((byte)localCharacterState.Length);
    for (int i = 0; i < localCharacterState.Length; i++)
    {
      localCharacterState[i].AppendToMessage(message);
    }
    message.AddByte((byte)localNetworkTransforms.Length);
    for (int i = 0; i < localNetworkTransforms.Length; i++)
    {
      localNetworkTransforms[i].AppendToMessage(message);
    }
    client.Send(message);
  }
}
