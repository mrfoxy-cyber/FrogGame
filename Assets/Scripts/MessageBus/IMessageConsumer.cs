using System;

public interface IMessageConsumer<T>
{
  void Consume(T message);
  Type GetMessageType();
}
