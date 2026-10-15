using System;
using System.Collections.Generic;
using UnityEngine;

public class MessageBusManager : MonoBehaviour, IMessageBus
{
  private static MessageBusManager _instance;
  public static MessageBusManager Instance
  {
    get => _instance;
    private set
    {
      if (_instance == null)
      {
        _instance = value;
      }
      else
      {
        Debug.LogError("Instance already set");
        Destroy(value);
      }
    }
  }

  public void Awake()
  {
    _instance = this;
  }

  private Dictionary<string, List<IMessageConsumer<object>>> _consumers = new Dictionary<string, List<IMessageConsumer<object>>>();
  private List<Tuple<string, Tuple<Type, object>>> _pendingMessages = new List<Tuple<string, Tuple<Type, object>>>();

  public void Attach(string queue, IMessageConsumer<object> messageConsumer)
  {
    lock (_consumers)
    {
      if (_consumers.ContainsKey(queue))
      {
        _consumers[queue].Add(messageConsumer);
      }
      else
      {
        _consumers.Add(queue, new List<IMessageConsumer<object>>() { messageConsumer });
      }
    }
  }


  public void Detach(string queue, IMessageConsumer<object> messageConsumer)
  {
    lock (_consumers)
    {
      if (_consumers.ContainsKey(queue))
      {
        _consumers[queue].Remove(messageConsumer);

        if (_consumers[queue].Count == 0)
        {
          _consumers.Remove(queue);
        }
      }
    }
  }

  public void Publish<T>(string queue, T message)
  {
    lock (_pendingMessages)
    {
      _pendingMessages.Add(Tuple.Create(queue, Tuple.Create(typeof(T), (object)message)));
    }
  }

  private void Update()
  {
    lock (_consumers)
    {
      lock (_pendingMessages)
      {
        for (int i = 0; i < _pendingMessages.Count; i++)
        {
          var queue = _pendingMessages[i].Item1;
          var type = _pendingMessages[i].Item2.Item1;
          var message = _pendingMessages[i].Item2.Item2;

          if (!_consumers.ContainsKey(queue))
          {
            Debug.LogError($"MessageBus error: A queue named '{queue}' is not attached");
            return;
          }

          for (int x = 0; x < _consumers[queue].Count; x++)
          {
            if (_consumers[queue][x].GetMessageType() == type)
            {
              _consumers[queue][x].Consume(message);
            }
          }
        }
        _pendingMessages.Clear();
      }
    }
  }
}
