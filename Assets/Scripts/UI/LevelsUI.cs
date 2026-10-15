using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelsUI : MonoBehaviour
{
  public Canvas canvas;
  public GameObject leveltemplatePrefab;
  public List<GameObject> buttons;

  // Start is called before the first frame update
  public void Awake()
  {


  }
  // Start is called before the first frame update
  void Start()
  {
    float x = 0;
    float i = 1;
    float j = 1;

    buttons = new List<GameObject>();

    foreach (KeyValuePair<int, Levels> level in GameItemsContainer.Instance.Levels)
    {
      var button = Instantiate(leveltemplatePrefab, canvas.transform);
      button.GetComponentInChildren<TextMeshProUGUI>().text = "Level" + i;
      button.name = i.ToString();
      button.GetComponent<Button>().image.sprite = level.Value.background;
      button.GetComponent<Button>().onClick.AddListener(buttonAction);
      button.transform.position = new Vector2(0.00f + x, 7f - 1f - j * 3f);
      button.transform.localScale = new Vector2(1, 1);
      buttons.Add(button);
      // Debug.Log(level.Value.goalPoints);
      i++;
      j++;
      if ((j - 1) % 3 == 0)
      {
        x = x + 3f;
        j = 1;
      }
    }

  }

  private void buttonAction()
  {
    PlayerPrefs.SetInt("ActiveLevel", int.Parse(EventSystem.current.currentSelectedGameObject.name));
    SceneManager.LoadScene("LevelTimeLimit");
  }

  // Update is called once per frame
  void Update()
  {
    int i = 0;
    foreach (KeyValuePair<int, Levels> level in GameItemsContainer.Instance.Levels)
    {
      buttons.ToArray()[i].GetComponent<LevelLock>().islocked = level.Value.locked;
      i++;
    }
  }
}
