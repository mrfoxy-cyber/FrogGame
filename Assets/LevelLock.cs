using UnityEngine;
using UnityEngine.UI;

public class LevelLock : MonoBehaviour
{
  public bool islocked;
  // Start is called before the first frame update
  void Start()
  {
    islocked = true;
  }

  // Update is called once per frame
  void Update()
  {
    if (islocked)
    {
      this.GetComponentInChildren<Animator>().enabled = false;
      this.GetComponent<Button>().interactable = false;
    }

    else
    {
      this.GetComponentInChildren<Animator>().enabled = true;
      this.GetComponent<Button>().interactable = true;
    }


  }

  void Open()
  {
    islocked = false;
  }
}
