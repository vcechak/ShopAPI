using Shop.Contracts;
using Shop.Contracts.Enums;
using Shop.DataAccess.Data;
using Shop.DataAccess.Data.Repository;
using Shop.Kafka.Messaging.Interfaces;

namespace Shop.Kafka.Consumer;

public class PaymentCheckWorker : BackgroundService
{
    private readonly IKafkaConsumer<PaymentCheckRequestMessage> _kafkaConsumer;
    private readonly IOrderRepository _orderRepository;

    public PaymentCheckWorker(IKafkaConsumer<PaymentCheckRequestMessage> kafkaConsumer, IOrderRepository orderRepository)
    {
        _kafkaConsumer = kafkaConsumer;
        _orderRepository = orderRepository;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("Starting consumer...");

        await _kafkaConsumer.ConsumeAsync("payments", async message =>
        {
            Console.WriteLine($"Processing payment for order: {message.OrderNumber}");

            // Simulate payment result we would get from bank api
            var isPaid = new Random().Next(0, 2) == 1; 
            var status = isPaid ? OrderState.Paid : OrderState.Cancelled;

            Console.WriteLine($"Order {message.OrderNumber} is {status}");

            // Update order status in the database
            var updated = await _orderRepository.UpdateStatusAsync(
                message.OrderNumber,
                status
            );

            if (updated)
            {
                Console.WriteLine($"Successfully updated order {message.OrderNumber} status.");
            }
            else
            {
                Console.WriteLine($"Failed to update order {message.OrderNumber} status.");
            }
        }, stoppingToken);
    }
}
