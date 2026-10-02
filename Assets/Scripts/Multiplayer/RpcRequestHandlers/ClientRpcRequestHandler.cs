using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class ClientRpcRequestHandler : MonoBehaviour
{
  private const float TickRate = 0.025f;
  private float _lastHandleTime = 0.0f;

  private Dictionary<ushort, Dictionary<ushort, MethodInfo>> _componentMethodCache = new Dictionary<ushort, Dictionary<ushort, MethodInfo>>();

  void Update()
  {
    if (Time.time > _lastHandleTime + TickRate)
    {
      _lastHandleTime = Time.time;
      var rpcRequests = ReceivedRpcRequestQueue.PopAllRequests();
      for (int i = 0; i < rpcRequests.Length; i++)
      {
        var component = RpcComponentCache.GetRpcComponentById(rpcRequests[i].ComponentId);
        if (component != null)
        {
          if (!_componentMethodCache.ContainsKey(rpcRequests[i].ComponentId))
          {
            _componentMethodCache.Add(rpcRequests[i].ComponentId, new Dictionary<ushort, MethodInfo>());
          }

          if (!_componentMethodCache[rpcRequests[i].ComponentId].ContainsKey(rpcRequests[i].MethodId))
          {
            var method = component
              .GetType()
              .GetMethods()
              .FirstOrDefault(x =>
                x.GetCustomAttributes(typeof(RpcMethodAttribute), false) is var attr
                  && attr.Length > 0
                  && ((RpcMethodAttribute)attr[0]).MethodId == rpcRequests[i].MethodId
              );

            if (method == null)
            {
              Debug.LogError($"Trying to execute RPC method with id {rpcRequests[i].MethodId} on comnponent with id {rpcRequests[i].ComponentId}. Method wasn't found" +
                $". Maybe the component overloads Start(). Should use Awake then instead. Cache base class uses start to register in cache.");
              return;
            }

            _componentMethodCache[rpcRequests[i].ComponentId].Add(rpcRequests[i].MethodId, method);
          }

          if (rpcRequests[i].ParameterCount > 0)
          {
            var parameterInfos = _componentMethodCache[rpcRequests[i].ComponentId][rpcRequests[i].MethodId].GetParameters();
            object[] parameters = null;
            try
            {
              parameters = rpcRequests[i].Parameters.Select((x, p) => ParseParameter(parameterInfos[p], x)).ToArray();
            }
            catch (Exception e)
            {
              Debug.LogError($"When parsing parameters for rpc request on method {_componentMethodCache[rpcRequests[i].ComponentId][rpcRequests[i].MethodId].Name} encountered an exception." +
                $"With parameters: {string.Join(", ", rpcRequests[i].Parameters)}");
            }

            try
            {
              _componentMethodCache[rpcRequests[i].ComponentId][rpcRequests[i].MethodId].Invoke(component, parameters);
            }
            catch (Exception e)
            {
              Debug.LogError($"Tried to make RPC method invocation on RPC component with name {RpcComponentCache.GetRpcComponentById(rpcRequests[i].ComponentId).gameObject.name}\n" +
                $"On method with name: {_componentMethodCache[rpcRequests[i].ComponentId][rpcRequests[i].MethodId].Name}" +
                $"With a parameter set: {string.Join(", ", parameters.Select(x => x.ToString()).ToArray())}" +
                $"An argument out of range exception occurred. Check if your target rpc method has the correct parameter count.");
            }
          }
          else
          {
            _componentMethodCache[rpcRequests[i].ComponentId][rpcRequests[i].MethodId].Invoke(component, new object[] { });
          }
        }
        else
        {
          Debug.LogError($"Trying to execute RPC on a component with id {rpcRequests[i].ComponentId} but a component with that Id is not registered");
        }
      }
    }
  }

  private static Dictionary<Type, Func<string, object>> typeParametersToObject = new Dictionary<Type, Func<string, object>>() {
    { typeof(ushort), (s) => ushort.Parse(s) },
    { typeof(int), (s) => int.Parse(s) },
    { typeof(string), (s) => s },
    { typeof(bool), (s) => bool.Parse(s) },
    { typeof(float), (s) => float.Parse(s) }
  };

  private static object ParseParameter(ParameterInfo parameterInfo, string parameter)
  {
    return typeParametersToObject[parameterInfo.ParameterType](parameter);
  }
}
