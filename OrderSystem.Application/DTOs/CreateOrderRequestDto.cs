using System.ComponentModel.DataAnnotations;

namespace OrderSystem.Application.DTOs
{
    public class CreateOrderRequestDto
    {
        [Required(ErrorMessage = "Order Number is required")]
        [StringLength(50, ErrorMessage = "Max length must be 50 characters")]
        public string OrderNumber { get; set; } = null!;

        [Required(ErrorMessage = "Total Amount is required")]
        public decimal TotalAmount { get; set; }
    }
}