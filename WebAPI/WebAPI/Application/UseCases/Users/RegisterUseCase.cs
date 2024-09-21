using WebAPI.Application.DTOs;
using WebAPI.Application.Interfaces.Services.Users;
using WebAPI.Application.Interfaces.UnitOfWork;
using WebAPI.Infrastructures.Interfaces;

namespace WebAPI.Application.UseCases.Users
{
    public class RegisterUseCase:IRegisterService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IPasswordHasher passwordHasher;
        private readonly IJwtProvider jwtProvider;
        public RegisterUseCase(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher, IJwtProvider jwtProvider)
        {
            this.unitOfWork = unitOfWork;
            this.passwordHasher = passwordHasher;
            this.jwtProvider = jwtProvider;
        }
        public async Task<int> Register(string userName, string email, string password)
        {
            var hashedPassword = passwordHasher.Generate(password);
            UserModel newUser = new();
            newUser.UserName = userName;
            newUser.Email = email;
            newUser.PasswordHash = hashedPassword;
            await unitOfWork.Users.AddNewUser(newUser);
            var result = await unitOfWork.SaveChangesAsync();
            return result;
        }
    }
}
