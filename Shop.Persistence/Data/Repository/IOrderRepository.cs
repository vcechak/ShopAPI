using Shop.Contracts.Enums;
using Shop.DataAccess.Models;

namespace Shop.DataAccess.Data.Repository;

public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(Guid id);

    Task<bool> UpdateStatusAsync(Guid id, OrderState newStatus);
}
