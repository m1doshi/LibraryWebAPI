using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Domain.Entities
{
    [Table("Users")]
    public class User
    {
        [Key]
        [Column("UserID")]
        public int UserID { get; set; }

        [Column("UserName")]
        public string UserName { get; set; }

        [Column("PasswordHash")]
        public string PasswordHash { get; set; }

        [Column("Email")]
        public string Email { get; set; }
    }
}
