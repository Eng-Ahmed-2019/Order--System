using System.ComponentModel.DataAnnotations;

namespace OrderSystem.Application.DTOs
{
    public class RegisterRequestDto
    {
        [Required(ErrorMessage = "Your name is required")]
        [StringLength(100, ErrorMessage = "FullName must be less than 100 characters")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "NationalId is required")]
        [StringLength(14, ErrorMessage = "NationalId must be 14 characters")]
        public string NationalId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Email address is not valid")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = string.Empty;
    }
}