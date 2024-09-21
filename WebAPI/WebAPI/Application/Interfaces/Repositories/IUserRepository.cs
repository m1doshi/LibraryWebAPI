using WebAPI.Application.DTOs;

namespace WebAPI.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<bool> AddNewUser(UserModel user);
        Task<UserModel> GetUserByEmail(string email);
        Task<UserModel> GetUserById(int userId);
    }
}
