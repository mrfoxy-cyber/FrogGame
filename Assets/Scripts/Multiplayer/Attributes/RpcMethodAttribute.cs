using System;

[AttributeUsage(AttributeTargets.Method)]
public class RpcMethodAttribute : Attribute
{
  public RpcMethodPriority RpcMethodPriority { get; set; }
  public byte MethodId { get; set; }

  public RpcMethodAttribute(RpcMethodPriority rpcMethodPriority, byte methodId)
  {
    this.RpcMethodPriority = RpcMethodPriority;
    this.MethodId = methodId;
  }
}
