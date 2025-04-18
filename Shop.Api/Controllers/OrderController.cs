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
    IKafkaProducer<PaymentRequestMessage> kafkaProducer,
    ShopDbContext dbContext) : ControllerBase
{
    private readonly IKafkaProducer<PaymentRequestMessage> _kafkaProducer = kafkaProducer;
    private readonly ShopDbContext _dbContext = dbContext;

    [HttpGet]
    public IActionResult GetOrders()
    {
        var orders = _dbContext.Orders.ToList();
        return Ok(orders);
    }

    [HttpGet("{id}")]
    public IActionResult GetOrder(Guid id)
    {
        var order = _dbContext.Orders
            .Include(o => o.OrderItems)
            .FirstOrDefault(o => o.Id == id);

        if (order == null)
        {
            return new NotFoundResult();
        }

        return Ok(order);
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrderAsync([FromBody] Order order)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        _dbContext.Orders.Add(order);
        _dbContext.SaveChanges();

        var total = order.OrderItems.Sum(i => i.Quantity * i.Price);

        // Send to Kafka
        var message = new PaymentRequestMessage
        {
            OrderNumber = order.Id,
            TotalAmount = total
        };

        await _kafkaProducer.ProduceAsync("payments", message, order.Id);

        return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
    }


    // Help endpoints
    [HttpDelete]
    public IActionResult DeleteAllOrders()
    {
        var orders = _dbContext.Orders.ToList();
        if (orders.Count == 0)
        {
            return NoContent();
        }
        _dbContext.Orders.RemoveRange(orders);
        _dbContext.SaveChanges();
        return Ok("All orders deleted successfully.");
    }

    [HttpGet("orderItems")]
    public IActionResult GetOrderItems()
    {
        var orderItems = _dbContext.OrderItems.ToList();
        return Ok(orderItems);
    }
}
