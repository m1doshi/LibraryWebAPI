using System.ComponentModel.DataAnnotations;
namespace WebAPI.Application.DTOs
{
    public class Role
    {
        public int RoleID { get; set; }

        [Required(ErrorMessage = "Role name is requierd.")]
        public string RoleName { get; set; }
    }
}
