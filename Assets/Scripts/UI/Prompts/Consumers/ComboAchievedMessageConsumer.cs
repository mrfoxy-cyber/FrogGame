public class ComboAchievedMessageConsumer : BaseMessageConsumer<ComboAchievedMessage>
{
  public override void Process(ComboAchievedMessage message)
  {
    PromptUIManager.Instance.CreateComboPrompt(message.UserID, message.Position, message.ComboPoints, message.ChainComboCounter, message.TotalPoints, message.BiggestCombo);
  }
}
