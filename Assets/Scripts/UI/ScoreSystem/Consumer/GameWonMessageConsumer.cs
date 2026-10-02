using Assets.Scripts.MessageBus.Messages;

public class GameWonMessageConsumer : BaseMessageConsumer<GameWonMessage>
{
  public override void Process(GameWonMessage message)
  {
    PromptUIManager.Instance.GameWon(message.GameWon, message.GoalPoints, message.TotalPoints);
  }
}
