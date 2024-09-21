using WebAPI.Models;

namespace WebAPI.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<int> Register(string userName, string email, string password);
        Task<string> Login(string email, string password);
    }
}
