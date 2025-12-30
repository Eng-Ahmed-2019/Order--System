using System.ComponentModel.DataAnnotations;

namespace OrderSystem.Application.DTOs
{
    public class ProcessPaymentRequestDto
    {
        [Required(ErrorMessage = "Order Id is required")]
        public int OrderId { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        public decimal Amount { get; set; }
    }
}