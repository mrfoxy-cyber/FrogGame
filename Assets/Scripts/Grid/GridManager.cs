using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Parameters;

public class GridManager : MonoBehaviour
{

  public GridUser griduser;

  private static GridManager _instance;

  [Header("Grid configuration")]
  [Range(1, 20)] public int GridTilesX;
  [Range(1, 20)] public int GridTilesY;
  public static GridManager Instance
  {
    get => _instance;
    private set
    {
      if (_instance == null)
      {
        _instance = value;
      }
      else
      {
        Debug.LogError("Instance already set");
        Destroy(value);
      }
    }
  }


  public void Awake()
  {
    _instance = this;
  }

  [Header("Prefabs")]
  public GridSlot GridSlotPrefab;
  public GameObject PointsUIPrefab;

  public int frame;




  [Header("Grid simulation configuration")]
  [Range(0.0f, 1.0f)] public float UpdateRate = 0.5f;

  [Range(0.0f, 10.0f)] public float TileCoefficientX = 0.5f;
  [Range(0.0f, 10.0f)] public float TileCoefficientY = 0.5f;

  private int oldGridTilesX, oldGridTilesY;
  private float oldCoefficientX, oldCoefficientY;

  private float lastUpdateTime;
  private int comboCounter;
  private int totalPoints;
  private int biggestCombo;
  private int combopointsCurrent;

  public GridSlot[,] instantiatedGridSlots;
  private bool checkCombos = false;

  void Start()
  {
    lastUpdateTime = Time.time;
    comboCounter = 0;
    totalPoints = 0;
    combopointsCurrent = 0;
    biggestCombo = 0;
    frame = 0;
    // InitializeGrid();
  }


  public void InitializeGrid(int GridTilesX, int GridTilesY)
  {
    instantiatedGridSlots = new GridSlot[GridTilesX, GridTilesY];

    for (int x = 0; x < GridTilesX; x++)
    {
      for (int y = 0; y < GridTilesY; y++)
      {
        var newGridSlot = Instantiate(GridSlotPrefab, transform);

        var xPosition = -(GridTilesX / 2.0f) + TileCoefficientX * x + 0.5f + this.transform.position.x;
        var yPosition = -(GridTilesY / 2.0f) + TileCoefficientY * y + 0.5f + this.transform.position.y;

        newGridSlot.transform.position = new Vector2(xPosition, yPosition);

        instantiatedGridSlots[x, y] = newGridSlot;
      }
    }
  }

  private void Update()
  {
    if (TileCoefficientX != oldCoefficientX || TileCoefficientY != oldCoefficientY || oldGridTilesX != GridTilesX || oldGridTilesY != GridTilesY)
    {
      //ClearGrid();
      InitializeGrid(GridTilesX, GridTilesY);
      oldCoefficientX = TileCoefficientX;
      oldCoefficientY = TileCoefficientY;
      oldGridTilesX = GridTilesX;
      oldGridTilesY = GridTilesY;
    }

    if (Time.time > lastUpdateTime + UpdateRate)
    {
      lastUpdateTime = Time.time;
      Step();
    }
  }

  private void Step()
  {
    for (int x = 0; x < GridTilesX; x++)
    {
      for (int y = GridTilesY - 1; y > 0; y--)
      {
        if (!instantiatedGridSlots[x, y].IsEmpty)
        {
          if (instantiatedGridSlots[x, y - 1].IsEmpty)
          {
            Parameters.FrogTypes type = instantiatedGridSlots[x, y].FrogType;
            instantiatedGridSlots[x, y].Empty();
            instantiatedGridSlots[x, y - 1].Fill(type);
            return;
          }
        }
      }
    }
    var comboSlots = ComboAssignment();

    bool isCombo = false;
    int comboPoints = 0;
    var xPosition = 0f;
    var yPosition = 0f;
    bool switchy = true;


    comboSlots.Where(x => x.IsCombo).ToList().ForEach(c =>
    {
      comboPoints += instantiatedGridSlots[c.X, c.Y].FrogSprites[instantiatedGridSlots[c.X, c.Y].FrogType].Points;
      instantiatedGridSlots[c.X, c.Y].Combo();
      if (switchy)
      {
        xPosition = instantiatedGridSlots[c.X, c.Y].transform.position.x;
        yPosition = instantiatedGridSlots[c.X, c.Y].transform.position.y;
        switchy = false;
      }
      isCombo = true;
    });

    if (isCombo)
    {
      Controls.pause = true;
      if (frame == 0)
      {
        comboCounter++;
        totalPoints += comboPoints * comboCounter;
        combopointsCurrent = comboPoints * comboCounter;

        if (combopointsCurrent > biggestCombo)
          biggestCombo = combopointsCurrent;

        MessageBusManager.Instance.Publish("root", new ComboAchievedMessage()
        {
          UserID = griduser.Id,
          ChainComboCounter = comboCounter,
          ComboPoints = comboPoints,
          Position = new Vector2(xPosition, yPosition),
          TotalPoints = totalPoints,
          BiggestCombo = biggestCombo
        });
      }
      StartCoroutine(Combo(comboSlots));
      frame++;

    }
    if (!isCombo)
    {
      combopointsCurrent = 0;
      comboCounter = 0;
      frame = 0;
      Controls.pause = false;
      FroggyPlacement();
    }

  }

  private void FroggyPlacement()
  {
    Parameters.FrogTypes type;
    Parameters.FrogTypes up;
    Parameters.FrogTypes down;
    Parameters.FrogTypes left;
    Parameters.FrogTypes right;

    for (int x = 0; x < GridTilesX; x++)
    {
      for (int y = GridTilesY - 1; y > -1; y--)
      {
        if (!instantiatedGridSlots[x, y].IsEmpty)
        {
          if (y == 0)
            down = FrogTypes.Empty;
          else
            down = instantiatedGridSlots[x, y - 1].FrogType;

          if (y == GridTilesY - 1)
            up = FrogTypes.Empty;
          else
            up = instantiatedGridSlots[x, y + 1].FrogType;

          if (x == GridTilesX - 1)
            right = FrogTypes.Empty;
          else
            right = instantiatedGridSlots[x + 1, y].FrogType;

          if (x == 0)
            left = FrogTypes.Empty;
          else
            left = instantiatedGridSlots[x - 1, y].FrogType;

          type = instantiatedGridSlots[x, y].FrogType;

          if (type == up && type == down && type == left && type == right)
            instantiatedGridSlots[x, y].Connect(instantiatedGridSlots[x, y].FrogType, Parameters.FroggyPlacement.LURD);
          else if (type == up && type == down && type == left && type != right)
            instantiatedGridSlots[x, y].Connect(instantiatedGridSlots[x, y].FrogType, Parameters.FroggyPlacement.LUD);
          else if (type == up && type == down && type != left && type == right)
            instantiatedGridSlots[x, y].Connect(instantiatedGridSlots[x, y].FrogType, Parameters.FroggyPlacement.RUD);
          else if (type == up && type != down && type == left && type == right)
            instantiatedGridSlots[x, y].Connect(instantiatedGridSlots[x, y].FrogType, Parameters.FroggyPlacement.RUL);
          else if (type != up && type == down && type == left && type == right)
            instantiatedGridSlots[x, y].Connect(instantiatedGridSlots[x, y].FrogType, Parameters.FroggyPlacement.RLD);
          else if (type == up && type == down && type != left && type != right)
            instantiatedGridSlots[x, y].Connect(instantiatedGridSlots[x, y].FrogType, Parameters.FroggyPlacement.UD);
          else if (type == up && type != down && type != left && type == right)
            instantiatedGridSlots[x, y].Connect(instantiatedGridSlots[x, y].FrogType, Parameters.FroggyPlacement.UR);
          else if (type != up && type != down && type == left && type == right)
            instantiatedGridSlots[x, y].Connect(instantiatedGridSlots[x, y].FrogType, Parameters.FroggyPlacement.RL);
          else if (type != up && type == down && type == left && type != right)
            instantiatedGridSlots[x, y].Connect(instantiatedGridSlots[x, y].FrogType, Parameters.FroggyPlacement.UD);
          else if (type != up && type == down && type != left && type == right)
            instantiatedGridSlots[x, y].Connect(instantiatedGridSlots[x, y].FrogType, Parameters.FroggyPlacement.DR);
          else if (type != up && type == down && type == left && type != right)
            instantiatedGridSlots[x, y].Connect(instantiatedGridSlots[x, y].FrogType, Parameters.FroggyPlacement.DL);
          else if (type == up && type != down && type == left && type != right)
            instantiatedGridSlots[x, y].Connect(instantiatedGridSlots[x, y].FrogType, Parameters.FroggyPlacement.UL);
          else if (type == up && type != down && type != left && type != right)
            instantiatedGridSlots[x, y].Connect(instantiatedGridSlots[x, y].FrogType, Parameters.FroggyPlacement.U);
          else if (type != up && type != down && type != left && type == right)
            instantiatedGridSlots[x, y].Connect(instantiatedGridSlots[x, y].FrogType, Parameters.FroggyPlacement.R);
          else if (type != up && type != down && type == left && type != right)
            instantiatedGridSlots[x, y].Connect(instantiatedGridSlots[x, y].FrogType, Parameters.FroggyPlacement.L);
          else if (type != up && type == down && type != left && type != right)
            instantiatedGridSlots[x, y].Connect(instantiatedGridSlots[x, y].FrogType, Parameters.FroggyPlacement.D);
          else
            instantiatedGridSlots[x, y].Connect(instantiatedGridSlots[x, y].FrogType, Parameters.FroggyPlacement.S);
          type = FrogTypes.Empty;
          up = FrogTypes.Empty;
          down = FrogTypes.Empty;
          left = FrogTypes.Empty;
          right = FrogTypes.Empty;

        }
      }
    }
  }

  private System.Collections.IEnumerator Combo(GridComboResult[] comboSlots)
  {
    yield return new WaitUntil(() => frame >= 10);
    comboSlots.Where(x => x.IsCombo).ToList().ForEach(c =>
    {
      instantiatedGridSlots[c.X, c.Y].Empty();
    });
    frame = 0;
  }

  private GridComboResult[] ComboAssignment()
  {
    var comboSlots =

      Enum.GetNames(typeof(FrogTypes))

      .SelectMany(f => Enumerable.Range(0, GridTilesX)

        .SelectMany(x => Enumerable.Range(0, GridTilesY)
          .Select(y =>
          {
            checkedIndices.Clear();
            return new GridComboResult()
            {
              IsCombo = ComboRecurssion((FrogTypes)Enum.Parse(typeof(FrogTypes), f), x, y) > 2,
              X = x,
              Y = y
            };
          }))
      )
      .ToArray();

    return comboSlots;
  }

  private List<Tuple<int, int>> checkedIndices = new List<Tuple<int, int>>();

  private int ComboRecurssion(FrogTypes frogType, int x, int y)
  {
    int total = 0;
    if (IsValidTileToRecurse(frogType, x, y))
    {
      checkedIndices.Add(Tuple.Create(x, y));
      for (int tx = -1; tx < 2; tx++)
      {
        for (int ty = -1; ty < 2; ty++)
        {
          int atTileX = x + tx;
          int atTileY = y + ty;

          if (!(atTileX < 0) && !(atTileY < 0) && !(atTileY > GridTilesY - 1) && !(atTileX > GridTilesX - 1))
          {
            if (ShouldRecursivelyVisitTile(frogType, x, y, tx, ty, atTileX, atTileY))
            {

              if (instantiatedGridSlots[atTileX, atTileY].FrogType == instantiatedGridSlots[x, y].FrogType)
              {
                total += 1;
                total += ComboRecurssion(frogType, atTileX, atTileY);
              }
            }
          }
        }
      }
    }
    return total;
  }

  private bool IsValidTileToRecurse(FrogTypes frogType, int x, int y)
    => !(frogType == FrogTypes.Empty || checkedIndices.Any(i => i.Item1 == x && i.Item2 == y));

  private bool ShouldRecursivelyVisitTile(FrogTypes frogType, int x, int y, int tx, int ty, int atTileX, int atTileY)
    => instantiatedGridSlots[atTileX, atTileY].FrogType == frogType
      && !((
      (tx == 0 && ty == 0)
      || (Mathf.Abs(tx) == 1 && ty != 0)
      || (Mathf.Abs(ty) == 1 && tx != 0)
      || x + tx > GridTilesX - 1 || x + tx < 0 || y + ty > GridTilesY - 1 || y + ty < 0)
      || checkedIndices.Any(i => i.Item1 == atTileX && i.Item2 == atTileY));
  public void Fill(int x, int y)
  {
    instantiatedGridSlots[x, y].Fill();
  }

}
