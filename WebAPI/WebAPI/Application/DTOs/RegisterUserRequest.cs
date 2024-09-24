using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Application.DTOs
{
    public class RegisterUserRequest
    {
        [Required(ErrorMessage = "Username is required")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Name length can't be more than 20 and less than 3.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(30, MinimumLength = 7, ErrorMessage = "Password length can't be more than 30 and less than 7.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [StringLength(40, MinimumLength = 6, ErrorMessage = "Email length can't be more than 40 and less than 6.")]
        public string Email { get; set; }
    }
}
