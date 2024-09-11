using WebAPI.Models;

namespace WebAPI.Services.Interfaces
{
    public interface IUserService
    {
        Task<int> Register(string userName, string email, string password);
        Task<string> Login(string email, string password);
    }
}
