using System;
using TheLab.Master.Contracts;
using UnityEngine;

namespace Assets.TheLab.Scripts.Api
{
  public class GroupApi : MonoBehaviour
  {
    private static GroupApi _instance;
    public static GroupApi Instance
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

    public void GetGroup(string token, Action<GetGroupResponse> responseCallback)
    {
      StartCoroutine(ApiUtility.Get(Constants.ApiRoute + Constants.GroupRoute + "/player/" + token, responseCallback));
    }
  }
}
