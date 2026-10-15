public interface IMessageBus
{
  void Attach(string queue, IMessageConsumer<object> messageBusConsumer);
  void Detach(string queue, IMessageConsumer<object> messageBusConsumer);
  void Publish<T>(string queue, T message);
}
