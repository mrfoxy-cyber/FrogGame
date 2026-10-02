namespace TheLab.Master.Contracts
{
  public class GetPlayerInventoryResponse
  {
    public GetPlayerInventoryResult Result { get; set; }
    public PlayerInventoryItem[] PlayerInventory { get; set; }
    public bool Success { get; set; }
  }
}
