using System.Security.Cryptography;
using WebAPI.Infrastructures.Interfaces;

namespace WebAPI.Infrastructure.Persistence
{
    public class RefreshProvider : IRefreshProvider
    {
        public string GenerateRefreshToken()
        {
            var refreshToken = new byte[32];
            using(var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(refreshToken);
                return Convert.ToBase64String(refreshToken);
            }
        }
    }
}
