using System.ComponentModel.DataAnnotations;

namespace Shop.Api.DTO
{
    public class NewOrderItemRequest
    {
        [Required]
        public string ProductName { get; set; } = string.Empty;

        [Required]
        public int Quantity { get; set; } = 0;

        [Required]
        public decimal Price { get; set; } = 0.0m;
    }
}
