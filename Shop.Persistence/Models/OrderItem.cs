using System.ComponentModel.DataAnnotations;

namespace Shop.DataAccess.Models;

public class OrderItem
{
    public Guid Id { get; set; } = new Guid();

    public string ProductName { get; set; } = string.Empty;

    public int Quantity { get; set; } = 0;

    public decimal Price { get; set; } = 0.0m;
}
