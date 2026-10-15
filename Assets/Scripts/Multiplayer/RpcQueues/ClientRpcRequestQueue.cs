using System.Collections.Generic;
public static class ClientRpcRequestQueue
{
  private static List<ClientRpcRequest> normalPriorityRequests = new List<ClientRpcRequest>();
  private static List<ClientRpcRequest> realtimePriorityRequests = new List<ClientRpcRequest>();

  public static void AddRequest(ClientRpcRequest rpcRequest, RpcMethodPriority rpcMethodPriority = RpcMethodPriority.Normal)
  {
    if (rpcMethodPriority == RpcMethodPriority.Normal)
    {
      lock (normalPriorityRequests)
      {
        normalPriorityRequests.Add(rpcRequest);
      }
    }
    else
    {
      lock (realtimePriorityRequests)
      {
        realtimePriorityRequests.Add(rpcRequest);
      }
    }
  }

  public static ClientRpcRequest[] PopAllRequests(RpcMethodPriority rpcMethodPriority = RpcMethodPriority.Normal)
  {
    if (rpcMethodPriority == RpcMethodPriority.Normal)
    {
      lock (normalPriorityRequests)
      {
        var requests = normalPriorityRequests.ToArray();

        normalPriorityRequests.Clear();

        return requests;
      }
    }
    else
    {
      lock (realtimePriorityRequests)
      {
        var requests = realtimePriorityRequests.ToArray();

        realtimePriorityRequests.Clear();

        return requests;
      }
    }
  }
}