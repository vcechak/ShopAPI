using Microsoft.AspNetCore.Mvc;
using Shop.Api.DTO;

[ApiController]
[Route("api/[controller]")]
public class OrderController(IOrderService orderService) : ControllerBase
{
    private readonly IOrderService _orderService = orderService;

    [HttpGet]
    public async Task<IActionResult> GetOrdersAsync()
    {
        var orders = await _orderService.GetOrdersAsync();
        return Ok(orders);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrderAsync(Guid id)
    {
        var order = await _orderService.GetOrderAsync(id);
        if (order == null)
        {
            return NotFound();
        }

        return Ok(order);
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrderAsync([FromBody] NewOrderRequest newOrderRequest)
    {
        var order = await _orderService.CreateOrderAsync(newOrderRequest);
        return StatusCode(StatusCodes.Status201Created, order);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteAllOrdersAsync()
    {
        var deleted = await _orderService.DeleteAllOrdersAsync();
        if (!deleted)
        {
            return NoContent();
        }

        return Ok("All orders deleted successfully.");
    }

    [HttpGet("orderItems")]
    public async Task<IActionResult> GetOrderItemsAsync()
    {
        var orderItems = await _orderService.GetOrderItemsAsync();
        return Ok(orderItems);
    }
}
