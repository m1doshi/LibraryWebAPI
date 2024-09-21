using WebAPI.Application.Interfaces.Services.Users;
using WebAPI.Application.Interfaces.UnitOfWork;
using WebAPI.Infrastructures.Interfaces;
using WebAPI.Infrastructures.Persistence;

namespace WebAPI.Application.UseCases.Users
{
    public class LoginUseCase:ILoginService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IPasswordHasher passwordHasher;
        private readonly IJwtProvider jwtProvider;
        public LoginUseCase(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher, IJwtProvider jwtProvider)
        {
            this.unitOfWork = unitOfWork;
            this.passwordHasher = passwordHasher;
            this.jwtProvider = jwtProvider;
        }
        public async Task<string> Login(string email, string password)
        {
            var user = await unitOfWork.Users.GetUserByEmail(email);
            var result = passwordHasher.Verify(password, user.PasswordHash);
            if (result == false) throw new Exception("Failed to login");
            var token = jwtProvider.GenerateToken(user);
            return token;
        }
    }
}
