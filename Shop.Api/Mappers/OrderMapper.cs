using Shop.Api.DTO;
using Shop.Contracts.Enums;
using Shop.DataAccess.Models;

namespace Shop.Api.Mappers;

public static class OrderMapper
{
    public static Order ToOrder(this NewOrderRequest request, string orderNumber)
    {
        return new Order
        {
            Id = Guid.NewGuid(),
            OrderNumber = orderNumber,
            CustomerName = request.CustomerName,
            CompanyName = request.CompanyName,
            OrderState = OrderState.New,
            OrderDate = request.OrderDate,
            OrderItems = request.OrderItems
                .Select(newOrderItem => newOrderItem.ToOrderItem())
                .ToList()
        };
    }
}
