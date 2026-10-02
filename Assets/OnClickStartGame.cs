using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OnClickStartGame : MonoBehaviour
{
  // Start is called before the first frame update

  void Awake()
  {
    this.GetComponent<Button>().onClick.AddListener(StartGame);
  }

  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {

  }

  void StartGame()
  {
    SceneManager.LoadScene("LevelTimeLimit");
  }
}
