using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
  public GameObject MenuButton;
  public Canvas Canvas;
  void Start()
  {

    Canvas = this.GetComponentInParent<Canvas>();

    var newgamebutton = Instantiate(MenuButton, Canvas.transform);
    newgamebutton.transform.position = new Vector2(0.02f, -1f);
    newgamebutton.transform.localScale = new Vector2(1, 1);
    newgamebutton.GetComponentInChildren<TextMeshProUGUI>().text = "New Game";
    newgamebutton.GetComponent<Button>().onClick.AddListener(NewGameAction);

    var loadgamebutton = Instantiate(MenuButton, Canvas.transform);
    loadgamebutton.transform.position = new Vector2(0.02f, -2.2f);
    loadgamebutton.transform.localScale = new Vector2(1, 1);
    loadgamebutton.GetComponentInChildren<TextMeshProUGUI>().text = "Load Game";
    loadgamebutton.GetComponent<Button>().onClick.AddListener(LoadGameAction);

#if !UNITY_WEBGL
    var battlebutton = Instantiate(MenuButton, Canvas.transform);
    battlebutton.transform.position = new Vector2(0.02f, -3.4f);
    battlebutton.transform.localScale = new Vector2(1, 1);
    battlebutton.GetComponentInChildren<TextMeshProUGUI>().text = "Battle";
    battlebutton.GetComponent<Button>().onClick.AddListener(BattleAction);
#endif

  }

  private void NewGameAction()
  {
    // MessageBusManager.Instance.Publish("root", new GameStartedMessage() { });


    PlayerPrefs.SetInt("ActiveLevel", 1);
#if UNITY_WEBGL
    // The desktop introduction uses a large AVI file, which is not suitable for a browser build.
    SceneManager.LoadScene("Levels");
#else
    SceneManager.LoadScene("Video");
#endif

    int i = 1;
    foreach (KeyValuePair<int, Levels> level in GameItemsContainer.Instance.Levels)
    {
      GameItemsContainer.Instance.Levels[i].locked = true;
      i++;
    }
    GameItemsContainer.Instance.Levels[1].locked = false;




  }

  private void LoadGameAction()
  {
    PlayerPrefs.SetInt("ActiveLevel", 1);
    SceneManager.LoadScene("Levels");
  }

  private void BattleAction()
  {
    SceneManager.LoadScene("MultiPlayer");
  }

  // Update is called once per frame
  void Update()
  {

  }


}
