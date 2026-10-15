using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OnClickMenu : MonoBehaviour
{
  // Start is called before the first frame update
  void Awake()
  {
    this.GetComponent<Button>().onClick.AddListener(Menu);
  }

  // Update is called once per frame
  void Update()
  {

  }

  void Menu()
  {
    SceneManager.LoadScene("StartMenu");
  }
}
