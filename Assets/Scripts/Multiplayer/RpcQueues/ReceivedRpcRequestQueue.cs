using System.Collections.Generic;
using System.Linq;

public static class ReceivedRpcRequestQueue
{
  private static List<ClientRpcRequest> rpcRequests = new List<ClientRpcRequest>();

  public static void AddRequest(ClientRpcRequest rpcRequest)
  {
    lock (rpcRequests)
    {
      rpcRequests.Add(rpcRequest);
    }
  }

  public static ClientRpcRequest[] PopAllRequests()
  {
    lock (rpcRequests)
    {
      var requests = rpcRequests.ToArray();

      rpcRequests.Clear();

      return requests;
    }
  }

  public static ClientRpcRequest PopRequest()
  {
    lock (rpcRequests)
    {
      var request = rpcRequests.Last();

      rpcRequests.Remove(request);

      return request;
    }
  }
}