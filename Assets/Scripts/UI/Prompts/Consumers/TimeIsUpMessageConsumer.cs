public class TimeIsUpMessageConsumer : BaseMessageConsumer<TimeIsUpMessage>
{
  public override void Process(TimeIsUpMessage message)
  {
    PromptUIManager.Instance.CreateTimeIsUp(message.Timeleft);
  }
}