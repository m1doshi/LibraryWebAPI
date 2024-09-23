using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebAPI.Domain.Exceptions;

namespace WebAPI.Domain.Entities
{   
    [Table("Roles")]
    public class Role
    {
        [Column("RoleID")]
        public int RoleID { get; set; }

        [Column("RoleName")]
        public string RoleName { get; set; }
    }
}
