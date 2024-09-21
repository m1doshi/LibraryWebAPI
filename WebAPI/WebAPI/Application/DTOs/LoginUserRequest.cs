using System.ComponentModel.DataAnnotations;

namespace WebAPI.Application.DTOs
{
    public class LoginUserRequest
    {
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
    }
}
