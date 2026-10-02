using System.Collections.Generic;
using System.Linq;

public static class ServerRpcRequestQueue
{
  private static List<ServerRpcRequest> rpcRequests = new List<ServerRpcRequest>();

  public static void AddRequest(ServerRpcRequest rpcRequest)
  {
    lock (rpcRequests)
    {
      rpcRequests.Add(rpcRequest);
    }
  }

  public static ServerRpcRequest[] PopAllRequests()
  {
    lock (rpcRequests)
    {
      var requests = rpcRequests.ToArray();

      rpcRequests.Clear();

      return requests;
    }
  }

  public static ServerRpcRequest PopRequest()
  {
    lock (rpcRequests)
    {
      var request = rpcRequests.Last();

      rpcRequests.Remove(request);

      return request;
    }
  }
}
