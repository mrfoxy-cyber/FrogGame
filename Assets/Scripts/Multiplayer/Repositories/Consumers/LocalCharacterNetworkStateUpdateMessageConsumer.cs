public class LocalCharacterNetworkStateUpdateMessageConsumer : BaseMessageConsumer<LocalCharacterNetworkStateUpdateMessage>
{
  public override void Process(LocalCharacterNetworkStateUpdateMessage message)
  {
    CharacterRepository.SetCharacterState(message.NetworkCharacterState);
  }
}
