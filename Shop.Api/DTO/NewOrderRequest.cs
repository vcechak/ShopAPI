using Shop.Contracts.Enums;
using Shop.DataAccess.Models;
using Shop.DataAccess.Validation;
using System.ComponentModel.DataAnnotations;

namespace Shop.Api.DTO;

[AtLeastOneRequired(nameof(CustomerName), nameof(CompanyName), ErrorMessage = "You must enter either Customer Name or Company Name.")]
public class NewOrderRequest
{
    [MaxLength(100)]
    public string CustomerName { get; set; } = string.Empty;

    [MaxLength(100)]
    public string CompanyName { get; set; } = string.Empty;

    [Required]
    public DateTime OrderDate { get; set; }

    [EnumerableNotEmpty]
    public List<NewOrderItemRequest> OrderItems { get; set; }
}
