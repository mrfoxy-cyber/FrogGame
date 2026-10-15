using RiptideNetworking;
using System.Collections.Generic;

public class ClientRpcRequest
{
  public bool Broadcast { get; set; }
  public ushort ClientId { get; set; }
  public ushort ComponentId { get; set; }
  public ushort MethodId { get; set; }
  public byte ParameterCount { get; set; }
  public List<string> Parameters { get; set; } = new List<string>();

  public static Message ToMessage(ClientRpcRequest rpcRequest)
  {
    var msg = Message.Create(MessageSendMode.reliable, (ushort)GameServerMessageType.ClientRpc);
    msg.AddBool(rpcRequest.Broadcast);
    if (!rpcRequest.Broadcast)
    {
      msg.AddUShort(rpcRequest.ClientId);
    }
    msg.AddUShort(rpcRequest.ComponentId);
    msg.AddUShort(rpcRequest.MethodId);
    msg.AddByte(rpcRequest.ParameterCount);
    for (int i = 0; i < rpcRequest.Parameters.Count; i++)
    {
      msg.AddStringASCII(rpcRequest.Parameters[i]);
    }
    return msg;
  }

  public static ClientRpcRequest FromMessage(Message msg)
  {
    var rpcRequest = new ClientRpcRequest();
    rpcRequest.Broadcast = msg.GetBool();
    if (!rpcRequest.Broadcast)
    {
      rpcRequest.ClientId = msg.GetUShort();
    }
    rpcRequest.ComponentId = msg.GetUShort();
    rpcRequest.MethodId = msg.GetUShort();
    rpcRequest.ParameterCount = msg.GetByte();
    for (int i = 0; i < rpcRequest.ParameterCount; i++)
    {
      var parameter = msg.GetStringASCII();
      rpcRequest.Parameters.Add(parameter);
    }
    return rpcRequest;
  }
}
