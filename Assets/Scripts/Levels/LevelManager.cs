using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
  private static LevelManager _instance;

  public Cloud cloud;
  public int frame;
  public GridManager grid;
  private bool cloudToHome;
  private Vector2 frogPos;
  public static LevelManager Instance
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
  public Image background;
  public int activeLevel;

  // Start is called before the first frame update
  public void Awake()
  {
    _instance = this;


  }

  void Start()
  {
    LoadLevel(PlayerPrefs.GetInt("ActiveLevel"));
    frame = 0;
  }

  public void LoadLevel(int level)
  {
    TimeCounter.Instance.ResetTime();
    GridSlotPreviewManager.Instance.NumberOfFrogtypes = GameItemsContainer.Instance.Levels[level].numberOfFrogs;
    PointsCounter.Instance.pointsGoal = GameItemsContainer.Instance.Levels[level].goalPoints;
    PointsCounter.Instance.leveltype = GameItemsContainer.Instance.Levels[level].leveltype;
    TimeCounter.Instance.maxtime = GameItemsContainer.Instance.Levels[level].timeLimit;
    background.sprite = GameItemsContainer.Instance.Levels[level].background;
    cloud.speed = GameItemsContainer.Instance.Levels[level].cloudSpeed;
  }

  // Update is called once per frame
  void Update()
  {



    if (!cloud.moving)
    {
      if (cloudToHome)
      {
        //cloud.Eat(GridManager.Instance.instantiatedGridSlots[(int)frogPos.x, (int)frogPos.y].FrogType);
        StartCoroutine(SendCloudToHome());
        frame++;
      }
      else
      {
        StartCoroutine(SendCloudToGrid());
      }

      frame++;
    }


  }

  private System.Collections.IEnumerator SendCloudToHome()
  {
    yield return new WaitUntil(() => frame >= 500);
    cloud.Home();
    cloud.Move();
    //  GridManager.Instance.instantiatedGridSlots[(int)frogPos.x, (int)frogPos.y].Empty();
    cloudToHome = false;
    frame = 0;
  }

  private System.Collections.IEnumerator SendCloudToGrid()
  {
    yield return new WaitUntil(() => frame >= 500);
    frogPos = cloud.ToGrid();
    cloud.Move();
    cloudToHome = true;
    frame = 0;
  }
}