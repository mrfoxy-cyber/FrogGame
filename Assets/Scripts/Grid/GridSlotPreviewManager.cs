using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Parameters;

public class GridSlotPreviewManager : MonoBehaviour
{
  private static GridSlotPreviewManager _instance;
  public static GridSlotPreviewManager Instance
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
    InitializeGrid();
  }

  public GridSlotPreview GridSlotPrefab;
  [Range(2, 10)] public int NumberOfFrogtypes;

  private List<GridSlotPreview> instantiatedGridSlots;

  public void InitializeGrid()
  {
    instantiatedGridSlots = new List<GridSlotPreview>();

    for (int x = 0; x < GridManager.Instance.GridTilesY; x++)
    {
      var newGridSlot = Instantiate(GridSlotPrefab, transform);
      newGridSlot.Fill((FrogTypes)Random.Range(1, NumberOfFrogtypes));
      instantiatedGridSlots.Add(newGridSlot);


    }
  }

  public void DrawGrid()
  {
    int x = 0;
    foreach (GridSlotPreview tile in instantiatedGridSlots)
    {
      var xPosition = -(GridManager.Instance.GridTilesX / 2.0f) - 0.75f + transform.position.x;
      var yPosition = -(GridManager.Instance.GridTilesY / 2.0f) + x + 0.5f + transform.position.y;
      tile.transform.position = new Vector2(xPosition, yPosition);
      x++;
    }
  }

  void Update()
  {
    DrawGrid();
  }

  public void ClearGrid()
  {
    if (instantiatedGridSlots != null)
    {
      foreach (var gridSlot in instantiatedGridSlots)
      {
        Destroy(gridSlot);
      }
    }

  }

  public FrogTypes Pop()
  {


    FrogTypes type = instantiatedGridSlots.First().FrogType;
    Destroy(instantiatedGridSlots.ElementAt(0).gameObject);
    instantiatedGridSlots.RemoveAt(0);
    var newGridSlot = Instantiate(GridSlotPrefab, transform);
    newGridSlot.Fill((FrogTypes)Random.Range(1, NumberOfFrogtypes + 1));
    instantiatedGridSlots.Add(newGridSlot);
    return type;
  }


  // Update is called once per frame
}
