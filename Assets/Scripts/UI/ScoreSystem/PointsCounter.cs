using Assets.Scripts.MessageBus.Messages;
using TMPro;
using UnityEngine;
using static Parameters;

public class PointsCounter : MonoBehaviour
{

  private static PointsCounter _instance;
  public float pointsGoal;
  public float currentPoints;
  public LevelType leveltype;
  public TextMeshProUGUI CurrentHealthText;
  private string starttext;

  private float basesize;
  public static PointsCounter Instance
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

  internal void UpdatePointsCounter(Vector2 position, int comboPoints, int chainComboCounter, int totalPoints, int biggestCombo)
  {


    if (leveltype == LevelType.MaxPoints)
      this.currentPoints = totalPoints;
    if (leveltype == LevelType.MaxCombo)
      this.currentPoints = biggestCombo;
  }

  public void Awake()
  {
    _instance = this;
  }



  // Start is called before the first frame update
  void Start()
  {
    basesize = GetComponent<RectTransform>().rect.height;
    //currentPoints = 50;
    if (leveltype == LevelType.MaxPoints)
      CurrentHealthText.text = "Points Goal (total points): " + pointsGoal;
    if (leveltype == LevelType.MaxCombo)
      CurrentHealthText.text = "Points Goal (combo): " + pointsGoal;


  }

  // Update is called once per frame
  void Update()
  {




    //CurrentHealthText.text = $"{starttext}:     {pointsGoal}\\{currentPoints}";
    if ((pointsGoal <= currentPoints && leveltype == LevelType.MaxPoints) ||
      (pointsGoal <= currentPoints && leveltype == LevelType.MaxCombo))
    {
      CurrentHealthText.text = "won";

      MessageBusManager.Instance.Publish("root", new GameWonMessage()
      {
        GameWon = true,
        TotalPoints = (int)currentPoints,
        GoalPoints = (int)pointsGoal
      });
    }
    float help = this.transform.localScale.y;
    if (currentPoints > 0)
    {
      RectTransform rt = this.GetComponent<RectTransform>();
      rt.sizeDelta = new Vector2(GetComponent<RectTransform>().rect.width, Mathf.Min(basesize * currentPoints / pointsGoal, basesize));
    }

  }
}
