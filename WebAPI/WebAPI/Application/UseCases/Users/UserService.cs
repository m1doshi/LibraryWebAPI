using WebAPI.Entities;
using WebAPI.Repositories.Interfaces;
using WebAPI.Application.Interfaces.Services;
using WebAPI.Application.Interfaces.UnitOfWork;
using WebAPI.Application.DTOs;
using WebAPI.Infrastructures.Interfaces;

namespace WebAPI.Application.UseCases.Users
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IPasswordHasher passwordHasher;
        private readonly IJwtProvider jwtProvider;

        public UserService(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher, IJwtProvider jwtProvider)
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
