using Shop.Contracts.Enums;
using Shop.DataAccess.Models;

namespace Shop.DataAccess.Data.Repository;

public class OrderRepository : IOrderRepository
{
    private readonly ShopDbContext _shopDbContext;

    public OrderRepository(ShopDbContext context)
    {
        _shopDbContext = context;
    }

    public async Task<Order?> GetByIdAsync(Guid id)
    {
        return await _shopDbContext.Orders.FindAsync(id);
    }

    public async Task<bool> UpdateStatusAsync(Guid id, OrderState newStatus)
    {
        var order = await _shopDbContext.Orders
            .FindAsync(id) ?? 
            throw new InvalidOperationException($"Order with ID {id} not found.");
        order.OrderState = newStatus;
        var response = await _shopDbContext.SaveChangesAsync();

        return response == 1;
    }
}
