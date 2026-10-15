using System.Collections.Generic;
using UnityEngine;
using static Parameters;
public class GridSlot : MonoBehaviour
{
  public Animator animator;
  public Sprite FilledSlotSprite;
  public Sprite EmptySlotSprite;
  public Sprite ComboSlotSprite;


  public Sprite S;
  public Sprite R;
  public Sprite L;
  public Sprite U;
  public Sprite D;
  public Sprite RL;
  public Sprite UL;
  public Sprite DL;
  public Sprite UD;
  public Sprite UR;
  public Sprite DR;
  public Sprite RUL;
  public Sprite RLD;
  public Sprite LUD;
  public Sprite RUD;
  public Sprite LURD;

  public Frog RedFrog;
  public Frog GreenFrog;
  public Frog YellowFrog;
  public Frog BlueFrog;
  public Frog WhiteFrog;
  public Frog BlackFrog;
  public Frog GreyFrog;
  public Frog PinkFrog;
  public Frog PurpleFrog;
  public Frog OrangeFrog;

  public float VelocityX;

  public int locx;
  public int locy;

  public bool IsEmpty { get; private set; }
  public bool IsCombo { get; private set; }

  public FrogTypes FrogType { get; private set; }
  public FroggyPlacement froggyPlacement { get; private set; }
  public Dictionary<FrogTypes, Frog> FrogSprites { get; private set; }
  public Dictionary<Parameters.FroggyPlacement, Sprite> FroggyPlacementSprites { get; private set; }

  private SpriteRenderer spriteRenderer;

  public string Ownergrid;

  private void Awake()
  {
    IsEmpty = true;
    spriteRenderer = GetComponent<SpriteRenderer>();
    animator = GetComponent<Animator>();
    FrogType = FrogTypes.Empty;
    Ownergrid = GetComponentInParent<GridManager>().name;


    FrogSprites = new Dictionary<FrogTypes, Frog>()
    {

    {FrogTypes.Black, BlackFrog},
    {FrogTypes.Red,   RedFrog},
    {FrogTypes.Blue,  BlueFrog},
    {FrogTypes.Green, GreenFrog},
    {FrogTypes.Pink, PinkFrog},
    {FrogTypes.Purple, PurpleFrog},
    {FrogTypes.Orange, OrangeFrog},
    {FrogTypes.Grey, GreyFrog},
    {FrogTypes.White, WhiteFrog},
    {FrogTypes.Yellow, YellowFrog},

    };

    FroggyPlacementSprites = new Dictionary<FroggyPlacement, Sprite>()
    {
      { FroggyPlacement.S,S },
      { FroggyPlacement.R,R },
      { FroggyPlacement.L,L },
      {FroggyPlacement.U,U },
      { FroggyPlacement.D,D },
      { FroggyPlacement.RL,RL },
      {FroggyPlacement.UL,UL },
      {FroggyPlacement.DL,DL },
      { FroggyPlacement.UD,UD },
      { FroggyPlacement.UR,UR },
      { FroggyPlacement.DR,DR },
      { FroggyPlacement.RUL,RUL },
      { FroggyPlacement.RLD,RLD },
      { FroggyPlacement.LUD,LUD },
      { FroggyPlacement.RUD,RUD },
      { FroggyPlacement.LURD,LURD }
    };
  }



  public void Fill()
  {
    this.FrogType = this.GetComponentInParent<GridUser>().gridSlotPreviewManager.Pop();
    spriteRenderer.sprite = ComboSlotSprite;
    spriteRenderer.color = FrogSprites[FrogType].Color;
    IsEmpty = false;
  }

  public void Fill(Parameters.FrogTypes type)
  {
    this.FrogType = type;

    spriteRenderer.sprite = ComboSlotSprite;
    spriteRenderer.color = FrogSprites[FrogType].Color;
    IsEmpty = false;
  }

  public void Empty()
  {
    spriteRenderer.sprite = EmptySlotSprite;
    spriteRenderer.color = Color.white;
    IsEmpty = true;
    IsCombo = false;
    animator.enabled = false;
    FrogType = FrogTypes.Empty;
  }

  public void Combo()
  {
    spriteRenderer.sprite = ComboSlotSprite;
    spriteRenderer.color = FrogSprites[FrogType].Color;
    animator.enabled = true;
    animator.Play("explodingfrogs");
    IsCombo = true;
  }

  public void Connect(Parameters.FrogTypes type, FroggyPlacement froggyPlacement)
  {
    this.froggyPlacement = froggyPlacement;
    if (froggyPlacement == Parameters.FroggyPlacement.R)
    {
      spriteRenderer.color = (Color)FrogSprites[type].Color;
      animator.enabled = true;
      animator.Play("Right");

    }
    else if (froggyPlacement == Parameters.FroggyPlacement.L)
    {
      spriteRenderer.color = (Color)FrogSprites[type].Color;
      animator.enabled = true;
      animator.Play("Left");
    }
    else if (froggyPlacement == Parameters.FroggyPlacement.U)
    {
      spriteRenderer.color = (Color)FrogSprites[type].Color;
      animator.enabled = true;
      animator.Play("Up");
    }
    else if (froggyPlacement == Parameters.FroggyPlacement.D)
    {
      spriteRenderer.color = (Color)FrogSprites[type].Color;
      animator.enabled = true;
      animator.Play("Down");
    }
    else
    {
      animator.enabled = false;
      spriteRenderer.sprite = FroggyPlacementSprites[froggyPlacement];
      spriteRenderer.color = (Color)FrogSprites[type].Color;
    }
    IsCombo = true;
  }
  public void CallCloud()
  {
    GameObject cloud = GameObject.Find("Cloud");
    cloud.GetComponent<Cloud>().movetox = transform.position.x;
    cloud.GetComponent<Cloud>().movetoy = transform.position.y;
  }
}
