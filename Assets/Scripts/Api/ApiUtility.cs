using System;
using System.Collections;
using Tiny;
using UnityEngine;
using UnityEngine.Networking;

public static class ApiUtility
{
  public static IEnumerator Get<Response>(string route, Action<Response> responseCallback)
  {
    Debug.Log(route);
    UnityWebRequest uwr = UnityWebRequest.Get(route);
    uwr.SetRequestHeader("Content-Type", "application/json");
    uwr.SetRequestHeader("Accept", "application/json");

    yield return uwr.SendWebRequest();

    if (uwr.result == UnityWebRequest.Result.DataProcessingError)
    {
      Debug.Log("Error on route " + route + "While Sending: " + uwr.error);
    }
    else
    {
      Debug.Log("Response: " + route + " - " + uwr.downloadHandler.text);

      responseCallback(Json.Decode<Response>(uwr.downloadHandler.text));
    }
  }

  public static IEnumerator Post<Request, Response>(string route, Request request, Action<Response> responseCallback)
  {
    UnityWebRequest uwr = UnityWebRequest.Put(route, Json.Encode(request));
    uwr.method = "POST";
    uwr.SetRequestHeader("Content-Type", "application/json");
    uwr.SetRequestHeader("Accept", "application/json");

    yield return uwr.SendWebRequest();

    if (uwr.result == UnityWebRequest.Result.DataProcessingError)
    {
      Debug.Log("Error While Sending: " + uwr.error);
    }
    else
    {
      responseCallback(Json.Decode<Response>(uwr.downloadHandler.text));
    }
  }

  public static IEnumerator Put<Request, Response>(string route, Request request, Action<Response> responseCallback)
  {
    UnityWebRequest uwr = UnityWebRequest.Put(route, Json.Encode(request));
    uwr.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(Json.Encode(request)));
    uwr.SetRequestHeader("Content-Type", "application/json");
    uwr.SetRequestHeader("Accept", "application/json");

    yield return uwr.SendWebRequest();

    if (uwr.result == UnityWebRequest.Result.DataProcessingError)
    {
      Debug.Log("Error While Sending: " + uwr.error);
    }
    else
    {
      responseCallback(Json.Decode<Response>(uwr.downloadHandler.text));
    }
  }
}
