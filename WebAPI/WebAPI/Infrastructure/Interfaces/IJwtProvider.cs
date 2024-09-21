using WebAPI.Application.DTOs;

namespace WebAPI.Infrastructures.Interfaces
{
    public interface IJwtProvider
    {
        string GenerateToken(UserModel user);
    }
}
