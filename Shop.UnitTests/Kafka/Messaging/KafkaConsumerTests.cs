using Confluent.Kafka;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shop.Kafka.Messaging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Shop.UnitAndIntegrationTests.Kafka.Messaging;

[TestClass]
public class KafkaConsumerTests
{
    [TestMethod]
    public void KafkaConsumer_ConsumeAsync_ShouldConsumeMessage()
    {
        // Arrange
        var config = new ConsumerConfig
        {
            BootstrapServers = "localhost:9092",
            GroupId = "test-consumer-group",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };
        var consumer = new KafkaConsumer<string>(config);
        string topic = "test-topic";
        string message = "Hello, Kafka!";
        var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(10));
        // Act
        Task.Run(() => consumer.ConsumeAsync(topic, msg =>
        {
            msg.Should().Be(message);
            return Task.CompletedTask;
        }, cancellationTokenSource.Token));
        // Assert
        // Add assertions to verify the message consumption
        // This is a placeholder. In a real test, you would need to produce a message to the topic
    }
}
