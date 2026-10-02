using System;
using TheLab.Master.Contracts;
using UnityEngine;

namespace Assets.TheLab.Scripts.Api
{
  public class InvitationsApi : MonoBehaviour
  {
    private static InvitationsApi _instance;
    public static InvitationsApi Instance
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

    public void SendInvite(string token, InvitationType invitationType, int playerId, Action<SendInviteResponse> responseCallback)
    {
      var request = new SendInviteRequest()
      {
        InvitePlayerId = playerId,
        Token = token.ToString(),
        Type = invitationType
      };

      StartCoroutine(ApiUtility.Post(Constants.ApiRoute + Constants.InvitationsRoute + "/invite", request, responseCallback));
    }

    public void GetPlayerInvitations(string token, InvitationType invitationType, Action<GetInvitationsResponse> responseCallback)
    {
      StartCoroutine(ApiUtility.Get(Constants.ApiRoute + Constants.InvitationsRoute + "/player/" + token.ToString() + "?invitationType=" + invitationType.ToString(), responseCallback));
    }

    public void AnswerInvitation(string token, int invitationId, Answer answer, Action<AnswerInvitationResponse> responseCallback)
    {
      var request = new AnswerInvitationRequest()
      {
        Answer = answer
      };

      StartCoroutine(ApiUtility.Post(Constants.ApiRoute + Constants.InvitationsRoute + "/player/" + token.ToString() + "/" + invitationId.ToString(), request, responseCallback));
    }
  }
}
