using System;
using TheLab.Master.Contracts;
using UnityEngine;

namespace Assets.TheLab.Scripts.Api
{
  public class PlayerApi : MonoBehaviour
  {
    private static PlayerApi _instance;
    public static PlayerApi Instance
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

    public void GetPlayerFriends(string token, Action<GetPlayerFriendsResponse> responseCallback)
    {
      StartCoroutine(ApiUtility.Get(Constants.ApiRoute + Constants.PlayerRoute + "/" + token.ToString() + "/friends", responseCallback));
    }

    public void SearchPlayers(string query, int page, Action<SearchPlayersResponse> responseCallback)
    {
      StartCoroutine(ApiUtility.Get(Constants.ApiRoute + Constants.PlayerRoute + $"?query={query}&page={page.ToString()}", responseCallback));
    }

    public void GetPlayerByToken(string token, Action<Player> responseCallback)
    {
      StartCoroutine(ApiUtility.Get(Constants.ApiRoute + Constants.PlayerRoute + "/" + token.ToString(), responseCallback));
    }

    public void GetPlayerById(int id, Action<Player> responseCallback)
    {
      StartCoroutine(ApiUtility.Get(Constants.ApiRoute + Constants.PlayerRoute + "/id/" + id.ToString(), responseCallback));
    }

    public void RegisterPlayerCoroutine(string name, Action<RegisterPlayerResponse> responseCallback)
    {
      var request = new RegisterPlayerRequest()
      {
        Name = name
      };
      StartCoroutine(ApiUtility.Post(Constants.ApiRoute + Constants.PlayerRoute, request, responseCallback));
    }

    public void SetPlayerName(string token, string name, Action<SetPlayerNameResponse> responseCallback)
    {
      var request = new SetPlayerNameRequest()
      {
        NewName = name
      };
      StartCoroutine(ApiUtility.Post(Constants.ApiRoute + Constants.PlayerRoute + "/" + token.ToString() + "/name/", request, responseCallback));
    }

    public void SetPlayerInformation(string token, string key, string value, Action<SetPLayerInformationResponse> responseCallback)
    {
      var request = new SetPlayerInformationRequest()
      {
        Key = key,
        Value = value
      };
      StartCoroutine(ApiUtility.Post(Constants.ApiRoute + Constants.PlayerRoute + "/" + token + "/info", request, responseCallback));
    }

    public void AddPlayerInventoryItem(string token, string category, string itemName, Action<AddPlayerInventoryItemResponse> responseCallback)
    {
      var request = new AddPlayerInventoryItemRequest()
      {
        Category = category,
        ItemName = itemName
      };
      StartCoroutine(ApiUtility.Post(Constants.ApiRoute + Constants.PlayerRoute + "/" + token + "/inventory", request, responseCallback));
    }

    public void GetPlayerInventory(string token, string category, Action<GetPlayerInventoryResponse> responseCallback)
    {
      StartCoroutine(ApiUtility.Get(Constants.ApiRoute + Constants.PlayerRoute + "/" + token + "/inventory", responseCallback));
    }
  }
}