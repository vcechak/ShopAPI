using Confluent.Kafka;
using System.Text.Json;
using Shop.Kafka.Messaging.Interfaces;

namespace Shop.Kafka.Messaging;

public class KafkaConsumer<T> : IKafkaConsumer<T>
{
    private readonly ConsumerConfig _config;

    public KafkaConsumer(ConsumerConfig config)
    {
        _config = config;
    }

    public async Task ConsumeAsync(string topic, Func<T, Task> handleMessage, CancellationToken cancellationToken)
    {
        using var consumer = new ConsumerBuilder<Ignore, string>(_config).Build();
        consumer.Subscribe(topic);
        Console.WriteLine($"Subscribing to topic {topic}");

        int i = 1;
        while (!cancellationToken.IsCancellationRequested)
        {
            Console.WriteLine($"Consume try: {i}");
            try
            {
                var result = consumer.Consume(TimeSpan.FromSeconds(5));

                if (result != null)
                {
                    var message = JsonSerializer.Deserialize<T>(result.Message.Value);

                    if (message is not null)
                    {
                        await handleMessage(message);
                    }
                    else
                    {
                        Console.WriteLine($"Failed to deserialize message: {result.Message.Value}");
                    }
                }
                else
                {
                    Console.WriteLine("No message consumed within the timeout period.");
                }
            }
            catch (ConsumeException e)
            {
                Console.WriteLine($"Kafka consume error: {e.Error.Reason}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }

            i++;
        }
    }
}
