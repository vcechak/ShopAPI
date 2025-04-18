namespace Shop.Kafka.Messaging.Interfaces;

public interface IKafkaConsumer<T>
{
    Task ConsumeAsync(string topic, Func<T, Task> handleMessage, CancellationToken cancellationToken);
}
