using WebAPI.Core.DTOs;

namespace WebAPI.Infrastructures.Interfaces
{
    public interface IJwtProvider
    {
        string GenerateToken(UserModel user);
    }
}
