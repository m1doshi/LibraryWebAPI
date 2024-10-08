using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Core.Entities
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

        [Column("RefreshToken")]
        public string? RefreshToken { get; set; }

        [Column("RefreshTokenExpireTime")]
        public DateTime RefreshTokenExpireTime { get; set; }

        [Column("RoleID")]
        public int RoleID { get; set; } = 2;
    }
}
