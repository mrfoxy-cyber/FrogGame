using TMPro;
using UnityEngine;

public class TimeCounter : MonoBehaviour
{
  private static TimeCounter _instance;
  public static TimeCounter Instance
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

  public TextMeshProUGUI timeCounter;
  public float starttime;
  public float maxtime;
  public float timeleft;

  // Start is called before the first frame update
  void Start()
  {
    ResetTime();
  }

  public void ResetTime()
  {
    starttime = Time.time;
  }

  // Update is called once per frame
  void Update()
  {
    timeleft = maxtime - (Time.time - this.starttime);
    timeCounter.text = ((int)(timeleft)).ToString() + " sek";

    MessageBusManager.Instance.Publish("root", new TimeIsUpMessage()
    {
      Timeleft = timeleft
    });
  }
}
