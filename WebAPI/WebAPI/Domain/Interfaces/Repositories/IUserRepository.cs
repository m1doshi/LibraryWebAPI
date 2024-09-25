using WebAPI.Application.DTOs;

namespace WebAPI.Domain.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<bool> AddNewUser(UserModel user);
        Task<UserModel> GetUserByEmail(string email);
        Task<UserModel> GetUserById(int userId);
        Task<UserModel> GetUserByRefreshToken(string refreshToken);
        Task<bool> UpdateUser(UserModel updatedUser);
    }
}
