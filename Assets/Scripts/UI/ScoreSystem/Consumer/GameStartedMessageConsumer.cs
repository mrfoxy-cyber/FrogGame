using Assets.Scripts.MessageBus.Messages;

public class GameStartedMessageConsumer : BaseMessageConsumer<GameStartedMessage>
{
  public override void Process(GameStartedMessage message)
  {
    TimeCounter.Instance.ResetTime();
  }
}
