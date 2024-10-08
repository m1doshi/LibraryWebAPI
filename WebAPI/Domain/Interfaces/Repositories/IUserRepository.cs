using WebAPI.Core.DTOs;
using WebAPI.Core.Entities;

namespace WebAPI.Core.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsers();
        Task<bool> AddNewUser(User user);
        Task<User> GetUserByEmail(string email);
        Task<User> GetUserById(int userId);
        Task<User> GetUserByRefreshToken(string refreshToken);
        Task<bool> UpdateUser(User user);
        Task<bool> DeleteUser(int userId);
    }
}
