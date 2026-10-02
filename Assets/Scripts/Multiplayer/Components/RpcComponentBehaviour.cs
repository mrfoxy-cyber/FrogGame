using UnityEngine;

public abstract class RpcComponentBehaviour : MonoBehaviour
{
  void Start()
  {
    Debug.Log($"Registering {GetRpcComponentId().ToString("X4")}");
    RpcComponentCache.Register(this);
  }

  private void OnDestroy()
  {
    Debug.Log($"Unregistering {GetRpcComponentId().ToString("X4")}");
    RpcComponentCache.Unregister(this);
  }

  public abstract ushort GetRpcComponentId();
}
