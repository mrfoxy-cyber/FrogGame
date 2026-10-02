using Assets.Scripts.MessageBus.Messages;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PromptUIManager : MonoBehaviour
{
  private static PromptUIManager _instance;
  public GameObject PointsUIPrefab;
  public GameObject TimeisUP;
  public GameObject GameisWon;
  public GameObject RestartButton;

  public Canvas Canvas;
  public TextMeshProUGUI ActiveLevel;
  private static bool switchy;

  public static PromptUIManager Instance
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
    switchy = true;
    ActiveLevel.text = "Level: " + PlayerPrefs.GetInt("ActiveLevel").ToString();
  }

  public void GameWon(bool gameWon, int goalPoints, int totalPoints)
  {

    if (switchy)
    {
      GameItemsContainer.Instance.Levels[PlayerPrefs.GetInt("ActiveLevel") + 1].locked = false;
      var timeisup = Instantiate(GameisWon, Canvas.transform);
      timeisup.transform.position = new Vector2(0.02f, 0.02f);
      timeisup.transform.localScale = new Vector2(150, 150);
      switchy = false;
      levelButtonsInitiation(true);
    }

  }

  private void levelButtonsInitiation(bool isGamewon)
  {

    List<GameObject> buttons = new List<GameObject>();

    var restart = Instantiate(RestartButton, Canvas.transform);
    restart.GetComponentInChildren<TextMeshProUGUI>().text = "Restart";
    restart.GetComponent<Button>().onClick.AddListener(RestartAction);
    buttons.Add(restart);

    var menubutton = Instantiate(RestartButton, Canvas.transform);
    menubutton.GetComponentInChildren<TextMeshProUGUI>().text = "Menu";
    menubutton.GetComponent<Button>().onClick.AddListener(MenuButtonAction);
    buttons.Add(menubutton);

    var prevbutton = Instantiate(RestartButton, Canvas.transform);
    prevbutton.GetComponentInChildren<TextMeshProUGUI>().text = "Previous Level";
    prevbutton.GetComponent<Button>().onClick.AddListener(PrevLevelAction);
    buttons.Add(prevbutton);

    var nextbutton = Instantiate(RestartButton, Canvas.transform);
    nextbutton.GetComponentInChildren<TextMeshProUGUI>().text = "Next Level";
    nextbutton.GetComponent<Button>().onClick.AddListener(NextLevelAction);
    buttons.Add(nextbutton);

    var l = PlayerPrefs.GetInt("ActiveLevel");

    if (l >= GameItemsContainer.Instance.Levels.Count || !isGamewon)
    {
      nextbutton.SetActive(false);
      Debug.Log("am i here active");
      buttons.Remove(nextbutton);
    }

    if (l <= 1)
    {
      prevbutton.SetActive(false);
      Debug.Log("am i here active");
      buttons.Remove(prevbutton);
    }

    float x = 1.2f;
    float i = 0;
    foreach (GameObject button in buttons)
    {
      button.transform.position = new Vector2(0.02f, -1f - i * 1.2f);
      button.transform.localScale = new Vector2(1, 1);
      i++;
    }
  }

  private void PrevLevelAction()
  {
    var l = PlayerPrefs.GetInt("ActiveLevel");
    PlayerPrefs.SetInt("ActiveLevel", l - 1);
    SceneManager.LoadScene("LevelTimeLimit");
  }



  internal void CreateTimeIsUp(float timeleft)
  {
    if (timeleft < 0 && switchy)
    {
      var timeisup = Instantiate(TimeisUP, Canvas.transform);
      timeisup.transform.position = new Vector2(0.02f, 0.02f);
      timeisup.transform.localScale = new Vector2(150, 150);

      levelButtonsInitiation(false);

      switchy = false;
    }
    //pointsui.transform.position = new Vector2(position.x, Mathf.Max(position.y, 0.02f));
  }

  public void MenuButtonAction()
  {
    SceneManager.LoadScene("StartMenu");
  }

  public void RestartAction()
  {
    MessageBusManager.Instance.Publish("root", new GameStartedMessage() { });
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
  }

  public void NextLevelAction()
  {
    var l = PlayerPrefs.GetInt("ActiveLevel");
    PlayerPrefs.SetInt("ActiveLevel", l + 1);
    SceneManager.LoadScene("LevelTimeLimit");
  }


  public void CreateComboPrompt(string userID, Vector2 position, int points, int counter, int totalPoints, int biggestCombo)
  {
    var pointsui = Instantiate(PointsUIPrefab, Canvas.transform);
    pointsui.GetComponentInChildren<TextMeshProUGUI>().text = "Wow " + points + " points";
    pointsui.transform.position = new Vector2(position.x, Mathf.Max(position.y, 0.02f));

    var pointsui2 = Instantiate(PointsUIPrefab, Canvas.transform);
    pointsui2.GetComponentInChildren<TextMeshProUGUI>().text = "Counter " + counter + "X";
    pointsui2.transform.position = new Vector2(3f, 4f);
    pointsui2.GetComponentInChildren<TextMeshProUGUI>().color = Color.red;
    //totalPoints += points * counter;
    GameObject.Find(userID).GetComponent<GridUser>().PointsText.text = totalPoints.ToString();
    GameObject.Find(userID).GetComponent<GridUser>().MaxComboPointsText.text = biggestCombo.ToString();

  }
}
