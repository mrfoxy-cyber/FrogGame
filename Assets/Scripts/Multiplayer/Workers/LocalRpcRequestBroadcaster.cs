using RiptideNetworking;

public static class LocalRpcRequestBroadcaster
{
  public static void BroadcastPendingRpcRequests(Client client)
  {
    BroadcastClientRpcRequests(client);
    BroadcastServerRpcRequests(client);
  }

  public static void BroadcastClientRpcRequests(Client client, RpcMethodPriority rpcMethodPriority = RpcMethodPriority.Normal)
  {
    var clientRpcRequests = ClientRpcRequestQueue.PopAllRequests(rpcMethodPriority);
    for (int i = 0; i < clientRpcRequests.Length; i++)
    {
      client.Send(ClientRpcRequest.ToMessage(clientRpcRequests[i]));
    }
  }

  private static void BroadcastServerRpcRequests(Client client)
  {
    var serverRpcRequests = ServerRpcRequestQueue.PopAllRequests();
    for (int i = 0; i < serverRpcRequests.Length; i++)
    {
      client.Send(ServerRpcRequest.ToMessage(serverRpcRequests[i]));
    }
  }
}
