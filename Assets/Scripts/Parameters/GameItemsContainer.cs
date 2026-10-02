using System.Collections.Generic;
using UnityEngine;

public class GameItemsContainer : MonoBehaviour
{
  // Start is called before the first frame update
  private static GameItemsContainer _instance;
  public Levels level1;
  public Levels level2;
  public Levels level3;
  public Levels level4;
  public Levels level5;
  public Levels level6;

  public Dictionary<int, Levels> Levels;
  public static GameItemsContainer Instance
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
  void Awake()
  {
    _instance = this;
    DontDestroyOnLoad(gameObject);
    Levels = new Dictionary<int, Levels>
    {
    {1, level1},
    {2,   level2},
    {3,  level3},
    {4, level4},
    {5,   level5},
    {6,  level6}
    };
  }
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {

  }
}
