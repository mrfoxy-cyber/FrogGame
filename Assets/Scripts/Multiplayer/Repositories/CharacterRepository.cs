using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class CharacterRepository
{
  private static Dictionary<ushort, NetworkCharacterState> _characterStates = new Dictionary<ushort, NetworkCharacterState>();

  public static void Clear()
  {
    lock (_characterStates)
    {
      _characterStates.Clear();
    }
  }

  public static NetworkCharacterState GetCharacterStateByGameServerCharacterId(ushort id)
  {
    lock (_characterStates)
    {
      if (!_characterStates.ContainsKey(id))
      {
        Debug.LogError($"Trying to retrieve character state for character with game server id {id} but state doesn't exist");
        return null;
      }

      return _characterStates[id];
    }
  }

  public static void SetCharacterState(NetworkCharacterState multiplayerCharacterState)
  {
    lock (_characterStates)
    {
      if (!_characterStates.ContainsKey(multiplayerCharacterState.CharacterId))
      {
        if (!multiplayerCharacterState.IsLocal)
        {
          MessageBusManager.Instance.Publish("root", new NetworkCharacterStateAddedMessage()
          {
            NetworkCharacterState = multiplayerCharacterState
          });
        }

        _characterStates.Add(multiplayerCharacterState.CharacterId, multiplayerCharacterState);
      }
      else
      {
        _characterStates[multiplayerCharacterState.CharacterId] = multiplayerCharacterState;
      }
    }

    if (!multiplayerCharacterState.IsLocal)
    {
      MessageBusManager.Instance.Publish("root", new NetworkCharacterLocalStateUpdateMessage()
      {
        CharacterState = multiplayerCharacterState
      });
    }
  }

  public static NetworkCharacterState[] GetLocalMultiplayerCharacterStates()
  {
    lock (_characterStates)
    {
      return _characterStates.Where(x => x.Value.IsLocal).Select(x => x.Value).ToArray();
    }
  }

  public static NetworkCharacterState[] GetRemoteMultiplayerCharacterStates()
  {
    lock (_characterStates)
    {
      return _characterStates.Where(x => !x.Value.IsLocal).Select(x => x.Value).ToArray();
    }
  }
}
