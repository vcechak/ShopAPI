using Shop.Api.DTO;
using Shop.DataAccess.Models;

namespace Shop.Api.Mappers
{
    public static class OrderItemMapper
    {
        public static OrderItem ToOrderItem(this NewOrderItemRequest newOrderItemRequest)
        {
            return new OrderItem
            {
                Id = Guid.NewGuid(),
                ProductName = newOrderItemRequest.ProductName,
                Quantity = newOrderItemRequest.Quantity,
                Price = newOrderItemRequest.Price
            };
        }
    }
}
