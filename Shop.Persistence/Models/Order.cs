using Shop.DataAccess.Validation;
using System.ComponentModel.DataAnnotations;
using Shop.Contracts.Enums;

namespace Shop.DataAccess.Models;

[AtLeastOneRequired(nameof(CustomerName), nameof(CompanyName), ErrorMessage = "You must enter either Customer Name or Company Name.")]
public class Order
{
    public Guid Id { get; set; } = new Guid();

    public string CustomerName { get; set; } = string.Empty;

    public string CompanyName { get; set; } = string.Empty;

    public OrderState OrderState { get; set; } = OrderState.New;

    [Required]
    public DateTime OrderDate { get; set; }

    [Required]
    public List<OrderItem> OrderItems { get; set; }
}
