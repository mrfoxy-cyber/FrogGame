using Assets.TheLab.Scripts.Api;
using System;
using TheLab.Master.Contracts;
using UnityEngine;

public class GameSettingsApi : MonoBehaviour
{
  private static GameSettingsApi _instance;
  public static GameSettingsApi Instance
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

  public void GetGameSettings(Action<GetGameSettingsResponse> responseCallback)
  {
    StartCoroutine(ApiUtility.Get(Constants.ApiRoute + Constants.GameSettingsRoute, responseCallback));
  }
}
