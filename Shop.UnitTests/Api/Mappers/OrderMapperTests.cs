
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shop.Api.DTO;
using Shop.Api.Mappers;
using System;
using System.Collections.Generic;

namespace Shop.UnitAndIntegrationTests.Api.Mappers;

[TestClass]
public class OrderMapperTests
{
    [TestMethod]
    public void ToOrder_ShouldMapNewOrderRequestToOrder()
    {
        // Arrange
        var newOrderRequest = new NewOrderRequest
        {
            CustomerName = "John Doe",
            CompanyName = "Doe Enterprises",
            OrderDate = DateTime.UtcNow,
            OrderItems = new List<NewOrderItemRequest>
            {
                new NewOrderItemRequest
                {
                    ProductName = "Product A",
                    Price = 10.99m,
                    Quantity = 2
                },
                new NewOrderItemRequest
                {
                    ProductName = "Product B",
                    Price = 5.49m,
                    Quantity = 1
                }
            }
        };

        // Act
        var orderNumber = "ORD123456";
        var order = newOrderRequest.ToOrder(orderNumber);
        order.Should().NotBeNull();
        order.OrderNumber.Should().Be(orderNumber);
        order.CustomerName.Should().Be(newOrderRequest.CustomerName);
        order.CompanyName.Should().Be(newOrderRequest.CompanyName);
        order.OrderDate.Should().Be(newOrderRequest.OrderDate);
        order.OrderItems.Should().NotBeNull();
        order.OrderItems.Should().HaveCount(newOrderRequest.OrderItems.Count);
        order.OrderItems[0].ProductName.Should().Be(newOrderRequest.OrderItems[0].ProductName);
        order.OrderItems[0].Price.Should().Be(newOrderRequest.OrderItems[0].Price);
        order.OrderItems[0].Quantity.Should().Be(newOrderRequest.OrderItems[0].Quantity);
        order.OrderItems[1].ProductName.Should().Be(newOrderRequest.OrderItems[1].ProductName);
        order.OrderItems[1].Price.Should().Be(newOrderRequest.OrderItems[1].Price);
        order.OrderItems[1].Quantity.Should().Be(newOrderRequest.OrderItems[1].Quantity);
        order.OrderState.Should().Be(Shop.Contracts.Enums.OrderState.New);
    }
}
