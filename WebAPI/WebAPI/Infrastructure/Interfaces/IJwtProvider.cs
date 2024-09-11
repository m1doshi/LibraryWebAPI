using WebAPI.Models;

namespace WebAPI.Infrastructure.Interfaces
{
    public interface IJwtProvider
    {
        string GenerateToken(UserModel user);
    }
}
