using System.ComponentModel.DataAnnotations;

namespace WebAPI.Application.DTOs
{
    public class LoginUserRequest
    {
        public string Password { get; set; }
        public string Email { get; set; }
    }
}
