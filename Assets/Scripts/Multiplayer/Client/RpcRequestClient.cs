using System.Linq;

public static class RpcRequestClient
{
  public static void ClientRpc(RpcComponentBehaviour rpcComponent, ushort clientId, ushort methodId, params string[] parameters)
  {
    ClientRpcRequestQueue.AddRequest(new ClientRpcRequest()
    {
      Broadcast = false,
      ClientId = clientId,
      ComponentId = rpcComponent.GetRpcComponentId(),
      MethodId = methodId,
      ParameterCount = (byte)parameters.Length,
      Parameters = parameters.ToList()
    });
  }

  public static void ClientRpc(RpcComponentBehaviour rpcComponent, ushort methodId, params string[] parameters)
  {
    ClientRpcRequestQueue.AddRequest(new ClientRpcRequest()
    {
      Broadcast = true,
      ComponentId = rpcComponent.GetRpcComponentId(),
      MethodId = methodId,
      ParameterCount = (byte)parameters.Length,
      Parameters = parameters.ToList()
    });
  }

  public static void ServerRpc(string methodName, params string[] parameters)
  {
    ServerRpcRequestQueue.AddRequest(new ServerRpcRequest()
    {
      FromClientId = GameServerClient.ClientId,
      MethodName = methodName,
      ParameterCount = (byte)parameters.Length,
      Parameters = parameters.ToList()
    });
  }
}
