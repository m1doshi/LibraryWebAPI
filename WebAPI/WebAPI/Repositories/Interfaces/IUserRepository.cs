using WebAPI.Models;
namespace WebAPI.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> AddNewUser(UserModel user);
        Task<UserModel> GetUserByEmail(string email);
        Task<UserModel> GetUserById(int userId);
    }
}
