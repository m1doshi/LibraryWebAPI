using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.DataAccess.Entities
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
