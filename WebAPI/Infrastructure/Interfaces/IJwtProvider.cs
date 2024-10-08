using WebAPI.Core.DTOs;
using WebAPI.Core.Entities;

namespace WebAPI.Infrastructures.Interfaces
{
    public interface IJwtProvider
    {
        string GenerateToken(User user);
    }
}
