using WebAPI.Application.DTOs;

namespace WebAPI.Application.Interfaces.Services.Users
{
    public interface ILoginService
    {
        Task<AuthenticationResponce> Login(string email, string password);
    }
}
