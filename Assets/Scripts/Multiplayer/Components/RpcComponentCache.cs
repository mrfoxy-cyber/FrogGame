using System.Collections.Generic;

public static class RpcComponentCache
{
  private static Dictionary<int, RpcComponentBehaviour> _rpcComponents = new Dictionary<int, RpcComponentBehaviour>();

  public static RpcComponentBehaviour GetRpcComponentById(int id)
  {
    if (!_rpcComponents.ContainsKey(id))
    {
      return null;
    }

    return _rpcComponents[id];
  }

  public static void Register(RpcComponentBehaviour rpcComponentBehaviour)
  {
    lock (_rpcComponents)
    {
      if (!_rpcComponents.ContainsKey(rpcComponentBehaviour.GetRpcComponentId()))
      {
        _rpcComponents.Add(rpcComponentBehaviour.GetRpcComponentId(), rpcComponentBehaviour);
      }
    }
  }

  public static void Unregister(RpcComponentBehaviour rpcComponentBehaviour)
  {
    lock (_rpcComponents)
    {
      _rpcComponents.Remove(rpcComponentBehaviour.GetRpcComponentId());
    }
  }
}
