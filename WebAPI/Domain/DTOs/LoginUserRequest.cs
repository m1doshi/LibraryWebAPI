using System.ComponentModel.DataAnnotations;

namespace WebAPI.Core.DTOs
{
    public class LoginUserRequest
    {
        public string Password { get; set; }
        public string Email { get; set; }
    }
}
