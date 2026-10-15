using RiptideNetworking;
using System.Collections.Generic;

public class ServerRpcRequest
{
  public ushort FromClientId { get; set; }
  public string MethodName { get; set; }
  public byte ParameterCount { get; set; }
  public List<string> Parameters { get; set; } = new List<string>();

  public static Message ToMessage(ServerRpcRequest serverRpcRequest)
  {
    var msg = Message.Create(MessageSendMode.reliable, (ushort)GameServerMessageType.ServerRpc);
    msg.AddUShort(serverRpcRequest.FromClientId);
    msg.AddStringASCII(serverRpcRequest.MethodName);
    msg.AddByte(serverRpcRequest.ParameterCount);
    for (int i = 0; i < serverRpcRequest.ParameterCount; i++)
    {
      msg.AddStringASCII(serverRpcRequest.Parameters[i]);
    }
    return msg;
  }

  public static ServerRpcRequest FromMessage(Message message)
  {
    var serverRpcRequest = new ServerRpcRequest();
    serverRpcRequest.FromClientId = message.GetUShort();
    serverRpcRequest.MethodName = message.GetStringASCII();
    serverRpcRequest.ParameterCount = message.GetByte();
    for (int i = 0; i < serverRpcRequest.ParameterCount; i++)
    {
      serverRpcRequest.Parameters.Add(message.GetStringASCII());
    }
    return serverRpcRequest;
  }
}