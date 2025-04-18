using System.ComponentModel.DataAnnotations;

namespace Shop.Contracts;

public class PaymentRequestMessage
{
    [Required]
    public Guid OrderNumber { get; set; } = default!;

    [Required]
    public decimal TotalAmount { get; set; }

    [Required]
    public bool Paid { get; set; } = false;
}
