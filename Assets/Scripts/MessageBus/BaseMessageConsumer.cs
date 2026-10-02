using System;
using UnityEngine;

public abstract class BaseMessageConsumer<T> : MonoBehaviour, IMessageConsumer<object>
{
  private Type _messageType = typeof(T);
  private string _queueName;

  public BaseMessageConsumer()
  {
    _queueName = "root";
  }

  public BaseMessageConsumer(string queueName)
  {
    this._queueName = queueName;
  }

  void Start()
  {
    MessageBusManager.Instance.Attach(_queueName, this);
  }

  private void OnDestroy()
  {
    MessageBusManager.Instance.Detach(_queueName, this);
  }

  public void Consume(object message)
  {
    var msgObj = (T)message;

    Process(msgObj);
  }

  public abstract void Process(T message);

  public Type GetMessageType()
  {
    return _messageType;
  }
}
