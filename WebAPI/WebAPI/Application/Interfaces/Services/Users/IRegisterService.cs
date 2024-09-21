namespace WebAPI.Application.Interfaces.Services.Users
{
    public interface IRegisterService
    {
        Task<int> Register(string userName, string email, string password);
    }
}
