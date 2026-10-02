using System;
using TheLab.Master.Contracts;
using UnityEngine;

namespace Assets.TheLab.Scripts.Api
{
  public class NotificationsApi : MonoBehaviour
  {
    private static NotificationsApi _instance;
    public static NotificationsApi Instance
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

    public void GetPlayerNotifications(string token, Action<GetNotificationsResponse> responseCallback, bool onlyUnread = false)
    {
      StartCoroutine(ApiUtility.Get(Constants.ApiRoute + Constants.NotificationsRoute + "/player/" + token.ToString() + (onlyUnread ? "onlyUnread=true" : ""), responseCallback));
    }

    public void GetPlayerUnreadNotificationsCount(string token, Action<GetUnreadNotificationsCountResponse> responseCallback)
    {
      StartCoroutine(ApiUtility.Get(Constants.ApiRoute + Constants.NotificationsRoute + "/player/" + token.ToString() + "/count", responseCallback));
    }

    public void MarkNotificationAsRead(string token, int notificationId, Action<MarkNotificationAsReadResponse> responseCallback)
    {
      var request = new MarkNotificationAsReadRequest()
      {
        NotificationId = notificationId,
        Token = token
      };
      StartCoroutine(ApiUtility.Post(Constants.ApiRoute + Constants.NotificationsRoute + "/player/" + token.ToString() + "/" + notificationId.ToString(), request, responseCallback));
    }

    public void MarkAllNotificationsAsRead(string token, Action<MarkAllNotificationsAsReadResponse> responseCallback)
    {
      var request = "";
      StartCoroutine(ApiUtility.Post(Constants.ApiRoute + Constants.NotificationsRoute + "/player/" + token.ToString(), request, responseCallback));
    }
  }
}
