namespace WebAPI.Core.DTOs
{
    public class UserModel
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpireTime { get; set; }
        public int RoleID { get; set; } = 2;
        public string RoleName { get; set; }
    }
}
