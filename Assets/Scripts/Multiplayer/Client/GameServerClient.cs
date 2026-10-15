using RiptideNetworking;
using RiptideNetworking.Transports.RudpTransport;
using RiptideNetworking.Utils;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(ClientRpcRequestHandler))]
public class GameServerClient : MonoBehaviour
{
  public bool TestInactivity = false;

  public static ushort ClientId;
  public static int GameId;

  private Client _client;
  private bool _run = true;

  [RuntimeInitializeOnLoadMethod]
  void RunOnStart()
  {
    Application.quitting += () =>
    {
      Quit();
    };
  }


  void Start()
  {
    CharacterRepository.Clear();
    NetworkTransformRepository.Clear();
    ClientRpcRequestQueue.PopAllRequests();
    ServerRpcRequestQueue.PopAllRequests();
    GameStateRepository.Reset();

    RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, true);
    Application.quitting += () => Quit();
    Task.Run(() =>
    {
      _client = new Client(new RudpClient(5000, 1000, 5, "CLIENT"));
      _client.Connected += (s, e) => OnClientConnected();
      _client.Disconnected += (s, e) => OnClientDisconnected(ClientId);
      _client.ClientDisconnected += (s, e) => OnClientDisconnected(e.Id);
      _client.Connect($"http://92.35.65.12:{GlobalSettings.GamePort}");

      while (_run)
      {
        if (!TestInactivity)
        {
          if (!_client.IsConnected && GameStateRepository.GetGameStateId() == 2)
          {
            OnClientDisconnected(ClientId);
            break;
          }

          LocalRpcRequestBroadcaster.BroadcastPendingRpcRequests(_client);
          LocalStateBroadcaster.BroadcastLocalState(_client);

          lock (_client)
          {
            _client.Tick();
          }
        }
        Thread.Sleep(48);
      }

      ServerRpcRequestQueue.AddRequest(new ServerRpcRequest()
      {
        FromClientId = ClientId,
        MethodName = "DestroyCharacter"
      });

      if (_client.IsConnected)
      {
        _client.Disconnect();
      }
    });
  }

  public void TestDisconnect()
  {
    _client.Disconnect();
  }

  void OnClientConnected()
  {
  }

  void OnClientDisconnected(ushort clientId)
  {
    Debug.Log($"Client disconnected: {clientId}");
    if (clientId == ClientId)
    {
      if (GameStateRepository.GetGameStateId() == 2)
      {
        MessageBusManager.Instance.Publish("root", new UnexpectedNetworkDisconnectedMessage() { });
      }
      else
      {
        MessageBusManager.Instance.Publish("root", new NetworkDisconnectedMessage() { });
      }

      Quit();
    }
  }

  private void OnDisable()
  {
    Quit();
  }

  void Quit()
  {
    _run = false;
  }

  [MessageHandler((ushort)GameServerMessageType.GameState)]
  static void HandleReceiveGameState(Message message)
  {
    var characterCount = message.GetByte();
    for (int i = 0; i < characterCount; i++)
    {
      var characterState = NetworkCharacterState.FromMessage(message);
      characterState.IsLocal = characterState.ClientId == ClientId;
      CharacterRepository.SetCharacterState(characterState);
    }
    var networkTransformCount = message.GetByte();
    for (int i = 0; i < networkTransformCount; i++)
    {
      var networkTransform = NetworkTransformState.FromMessage(message);
      NetworkTransformRepository.SetNetworkTransformState(networkTransform);
    }
  }

  [MessageHandler((ushort)GameServerMessageType.ClientRpc)]
  static void HandleClientRpcRequest(Message message)
  {
    Debug.Log($"Received a client rpc");
    ReceivedRpcRequestQueue.AddRequest(ClientRpcRequest.FromMessage(message));
  }

  public void TestSpawnRequest()
  {
    ServerRpcRequestQueue.AddRequest(new ServerRpcRequest()
    {
      FromClientId = ClientId,
      MethodName = "SpawnCharacter"
    });
  }

  [MessageHandler((ushort)GameServerMessageType.ClientConnectionConfirmation)]
  static void HandleClientConnectionConfirmationRequest(Message message)
  {
    ClientId = message.GetUShort();
    Debug.Log($"Received client id: {ClientId}");
    GameId = message.GetInt();
    Debug.Log($"Received game id: {GameId}");

    MessageBusManager.Instance.Publish("root", new GameServerIdReceivedMessage()
    {
      GameServerId = GameId
    });

    ServerRpcRequestQueue.AddRequest(new ServerRpcRequest()
    {
      FromClientId = ClientId,
      MethodName = "SpawnCharacter"
    });
  }
}
