using System;
using TheLab.Master.Contracts;
using UnityEngine;

namespace Assets.TheLab.Scripts.Api
{
  public class GameApi : MonoBehaviour
  {
    private static GameApi _instance;
    public static GameApi Instance
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

    public void GetGameServer(string gameMode, Action<GetGameServerResponse> responseCallback)
    {
      StartCoroutine(ApiUtility.Get<GetGameServerResponse>(Constants.ApiRoute + Constants.GameDiscoveryRoute + "/lobby?gameMode=" + gameMode, responseCallback));
    }

    public void GetGameScores(int gameId, Action<GetGameScoreResultsReponse> responseCallback)
    {
      StartCoroutine(ApiUtility.Get<GetGameScoreResultsReponse>(Constants.ApiRoute + Constants.GameDiscoveryRoute + "/scores?gameId=" + gameId, responseCallback));
    }

    public void GetGameHighScores(string gameMode, Action<GetHighScoresByGameModeResponse> responseCallback)
    {
      StartCoroutine(ApiUtility.Get<GetHighScoresByGameModeResponse>(Constants.ApiRoute + Constants.GameDiscoveryRoute + "/scores/top?gameMode=" + gameMode, responseCallback));
    }
  }
}
