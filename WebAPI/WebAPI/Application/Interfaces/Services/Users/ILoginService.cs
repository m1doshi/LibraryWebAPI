namespace WebAPI.Application.Interfaces.Services.Users
{
    public interface ILoginService
    {
        Task<string> Login(string email, string password);
    }
}
