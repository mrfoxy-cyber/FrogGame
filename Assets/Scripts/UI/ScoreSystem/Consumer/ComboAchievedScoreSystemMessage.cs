public class ComboAchievedScoreSystemMessage : BaseMessageConsumer<ComboAchievedMessage>
{
  public override void Process(ComboAchievedMessage message)
  {
    PointsCounter.Instance.UpdatePointsCounter(message.Position, message.ComboPoints, message.ChainComboCounter, message.TotalPoints, message.BiggestCombo);
  }
}
