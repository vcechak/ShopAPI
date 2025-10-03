using Shop.Api.DTO;
using Shop.DataAccess.Models;

public interface IOrderService
{
    Task<List<Order>> GetOrdersAsync();

    Task<Order?> GetOrderAsync(Guid id);

    Task<Order> CreateOrderAsync(NewOrderRequest newOrderRequest);

    Task<bool> DeleteAllOrdersAsync();

    Task<List<OrderItem>> GetOrderItemsAsync();
}
