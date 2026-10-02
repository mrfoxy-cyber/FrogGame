public class ServerRpcRequestPublisher : RpcComponentBehaviour
{
  public override ushort GetRpcComponentId()
  {
    return 0xF003;
  }

  [RpcMethod(RpcMethodPriority.Normal, 0)]
  public void PublishGameStateUpdatedMessage(int gameState)
  {
    GameStateRepository.SetGameStateId(gameState);

    MessageBusManager.Instance.Publish("root", new MultiplayerGameStateChangedMessage()
    {
      GameState = gameState
    });
  }
}
