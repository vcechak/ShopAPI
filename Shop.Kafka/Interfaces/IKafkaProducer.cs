namespace Shop.Kafka.Messaging.Interfaces;

public interface IKafkaProducer<T>
{
    Task ProduceAsync(string topic, T message, Guid key);
}
