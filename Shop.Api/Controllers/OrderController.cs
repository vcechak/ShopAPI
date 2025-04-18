using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Contracts;
using Shop.DataAccess.Data;
using Shop.DataAccess.Models;
using Shop.Kafka.Messaging.Interfaces;

namespace Shop.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController(
    IKafkaProducer<PaymentCheckRequestMessage> kafkaProducer,
    ShopDbContext dbContext) : ControllerBase
{
    private readonly IKafkaProducer<PaymentCheckRequestMessage> _kafkaProducer = kafkaProducer;
    private readonly ShopDbContext _dbContext = dbContext;

    [HttpGet]
    public async Task<IActionResult> GetOrdersAsync()
    {
        var orders = await _dbContext.Orders.ToListAsync();
        return Ok(orders);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrderAsync(Guid id)
    {
        var order = await _dbContext.Orders
            .Include(o => o.OrderItems)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null)
        {
            return NotFound();
        }

        return Ok(order);
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrderAsync([FromBody] Order order)
    {
        _dbContext.Orders.Add(order);
        await _dbContext.SaveChangesAsync();

        var total = order.OrderItems.Sum(i => i.Quantity * i.Price);

        // Send payment check request:
        var message = new PaymentCheckRequestMessage
        {
            OrderNumber = order.Id,
            TotalAmount = total
        };

        await _kafkaProducer.ProduceAsync("payments", message, order.Id);

        return CreatedAtAction(nameof(GetOrderAsync), new { id = order.Id }, order);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteAllOrdersAsync()
    {
        var orders = await _dbContext.Orders.ToListAsync();
        if (orders.Count == 0)
        {
            return NoContent();
        }
        _dbContext.Orders.RemoveRange(orders);
        await _dbContext.SaveChangesAsync();
        return Ok("All orders deleted successfully.");
    }

    [HttpGet("orderItems")]
    public async Task<IActionResult> GetOrderItemsAsync()
    {
        var orderItems = await _dbContext.OrderItems.ToListAsync();
        return Ok(orderItems);
    }
}