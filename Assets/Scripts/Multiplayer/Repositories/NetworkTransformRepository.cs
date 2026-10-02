using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class NetworkTransformRepository
{
  private static Dictionary<ushort, NetworkTransformState> _networkTransforms = new Dictionary<ushort, NetworkTransformState>();

  public static void Clear()
  {
    lock (_networkTransforms)
    {
      _networkTransforms.Clear();
    }
  }

  public static NetworkTransformState GetNetworkTransformStateById(ushort id)
  {
    lock (_networkTransforms)
    {
      if (!_networkTransforms.ContainsKey(id))
      {
        Debug.LogError($"Trying to retrieve NetworkTransformState with id {id} but state doesn't exist");
        return null;
      }

      return _networkTransforms[id];
    }
  }

  public static void SetNetworkTransformState(NetworkTransformState networkTransformState)
  {
    lock (_networkTransforms)
    {
      if (!_networkTransforms.ContainsKey(networkTransformState.NetworkTransformId))
      {
        _networkTransforms.Add(networkTransformState.NetworkTransformId, networkTransformState);
      }
      else
      {
        _networkTransforms[networkTransformState.NetworkTransformId] = networkTransformState;
      }
    }
  }

  public static NetworkTransformState[] GetLocalNetworkTransformStates()
  {
    lock (_networkTransforms)
    {
      return _networkTransforms.Where(x => x.Value.IsLocal).Select(x => x.Value).ToArray();
    }
  }

  public static NetworkTransformState[] GetRemoteMultiplayerCharacterStates()
  {
    lock (_networkTransforms)
    {
      return _networkTransforms.Where(x => !x.Value.IsLocal).Select(x => x.Value).ToArray();
    }
  }
}
