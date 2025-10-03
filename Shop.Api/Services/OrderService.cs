using Microsoft.EntityFrameworkCore;
using Shop.Api.DTO;
using Shop.DataAccess.Data;
using Shop.DataAccess.Models;
using Shop.Kafka.Messaging.Interfaces;
using Shop.Contracts;

public class OrderService : IOrderService
{
    private readonly IKafkaProducer<PaymentCheckRequestMessage> _kafkaProducer;
    private readonly ShopDbContext _dbContext;
    private readonly IOrderNumberGenerator _orderNumberGenerator;

    public OrderService(
        IKafkaProducer<PaymentCheckRequestMessage> kafkaProducer,
        ShopDbContext dbContext,
        IOrderNumberGenerator orderNumberGenerator)
    {
        _kafkaProducer = kafkaProducer;
        _dbContext = dbContext;
        _orderNumberGenerator = orderNumberGenerator;
    }

    public async Task<List<Order>> GetOrdersAsync()
    {
        return await _dbContext.Orders.ToListAsync();
    }

    public async Task<Order?> GetOrderAsync(Guid id)
    {
        return await _dbContext.Orders
            .Include(o => o.OrderItems)
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<Order> CreateOrderAsync(NewOrderRequest newOrderRequest)
    {
        var orderNumber = await _orderNumberGenerator.GenerateOrderNumberAsync();
        var order = newOrderRequest.ToOrder(orderNumber);

        _dbContext.Orders.Add(order);
        await _dbContext.SaveChangesAsync();

        var total = order.OrderItems.Sum(i => i.Quantity * i.Price);

        var message = new PaymentCheckRequestMessage
        {
            OrderNumber = order.Id,
            TotalAmount = total
        };

        await _kafkaProducer.ProduceAsync("payments", message, order.Id);

        return order;
    }

    public async Task<bool> DeleteAllOrdersAsync()
    {
        var orders = await _dbContext.Orders.ToListAsync();
        if (orders.Count == 0)
            return false;

        _dbContext.Orders.RemoveRange(orders);
        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<List<OrderItem>> GetOrderItemsAsync()
    {
        return await _dbContext.OrderItems.ToListAsync();
    }
}
