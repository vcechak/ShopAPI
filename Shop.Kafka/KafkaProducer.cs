using Confluent.Kafka;
using System.Text.Json;
using Shop.Kafka.Messaging.Interfaces;

namespace Shop.Kafka.Messaging;

public class KafkaProducer<T>(ProducerConfig config) : IKafkaProducer<T>
{
    private readonly IProducer<string, string> _producer = new ProducerBuilder<string, string>(config).Build();

    public async Task ProduceAsync(string topic, T message, Guid key)
    {
        var value = JsonSerializer.Serialize(message);
        var result = await _producer.ProduceAsync(
            topic, 
            new Message<string, string> 
            { 
                Key = key.ToString(),
                Value = value 
            });
        Console.WriteLine($"Message produced to topic {result.TopicPartitionOffset}");
    }
}