using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shop.Api.DTO;
using Shop.Api.Mappers;

namespace Shop.UnitAndIntegrationTests.Api.Mappers;

[TestClass]
public class OrderItemMapperTests
{
    [TestMethod]
    public void ToOrderItem_ShouldMapNewOrderItemRequestToOrderItem()
    {
        // Arrange
        var newOrderItemRequest = new NewOrderItemRequest
        {
            ProductName = "Product A",
            Price = 10.99m,
            Quantity = 2
        };
        
        // Act
        var orderItem = newOrderItemRequest.ToOrderItem();
        
        // Assert
        orderItem.Should().NotBeNull();
        orderItem.ProductName.Should().Be(newOrderItemRequest.ProductName);
        orderItem.Price.Should().Be(newOrderItemRequest.Price);
        orderItem.Quantity.Should().Be(newOrderItemRequest.Quantity);
    }
}
