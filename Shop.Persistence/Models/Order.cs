﻿using Shop.DataAccess.Validation;
using System.ComponentModel.DataAnnotations;
using Shop.Contracts.Enums;

namespace Shop.DataAccess.Models;

[AtLeastOneRequired(nameof(CustomerName), nameof(CompanyName), ErrorMessage = "You must enter either Customer Name or Company Name.")]
public class Order
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    public string OrderNumber { get; set; }

    [MaxLength(100)]
    public string CustomerName { get; set; } = string.Empty;

    [MaxLength(100)]
    public string CompanyName { get; set; } = string.Empty;

    [Required]
    public OrderState OrderState { get; set; } = OrderState.New;

    [Required]
    public DateTime OrderDate { get; set; }

    [EnumerableNotEmpty]
    public List<OrderItem> OrderItems { get; set; }
}
