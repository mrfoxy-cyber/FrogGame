using System.Collections.Generic;
using UnityEngine;
using static Parameters;

public class GridSlotPreview : MonoBehaviour
{
  private SpriteRenderer spriteRenderer;

  public Sprite FilledSlotSprite;
  public Sprite EmptySlotSprite;
  public Sprite ComboSlotSprite;
  public GridSlot gridSlot;

  private Dictionary<FrogTypes, Sprite> FrogSprites;



  public FrogTypes FrogType { get; set; }


  // Start is called before the first frame update
  void Awake()
  {
    spriteRenderer = GetComponent<SpriteRenderer>();

    FrogSprites = new Dictionary<FrogTypes, Sprite>()
    {

    {FrogTypes.Black, gridSlot.BlackFrog.FrogSprite},
    {FrogTypes.Red, gridSlot.RedFrog.FrogSprite},
    {FrogTypes.Blue, gridSlot.BlueFrog.FrogSprite},
    {FrogTypes.Green, gridSlot.GreenFrog.FrogSprite},
    {FrogTypes.Pink, gridSlot.PinkFrog.FrogSprite},
    {FrogTypes.Purple, gridSlot.PurpleFrog.FrogSprite},
    {FrogTypes.Orange, gridSlot.OrangeFrog.FrogSprite},
    {FrogTypes.Grey, gridSlot.GreyFrog.FrogSprite},
    {FrogTypes.White, gridSlot.WhiteFrog.FrogSprite},
    {FrogTypes.Yellow, gridSlot.YellowFrog.FrogSprite},

    };


  }

  public void Fill(Parameters.FrogTypes type)
  {
    this.FrogType = type;

    spriteRenderer.sprite = FrogSprites[type];
  }
}
