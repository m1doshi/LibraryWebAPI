using WebAPI.Core.DTOs;
using WebAPI.Core.Interfaces.UnitOfWork;
using WebAPI.Infrastructures.Interfaces;

namespace WebAPI.Application.UseCases.Users
{
    public class RegisterUseCase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IPasswordHasher passwordHasher;
        public RegisterUseCase(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher)
        {
            this.unitOfWork = unitOfWork;
            this.passwordHasher = passwordHasher;
        }
        public async virtual Task<int> Register(string userName, string email, string password)
        {
            var hashedPassword = passwordHasher.Generate(password);
            UserModel newUser = new();
            newUser.UserName = userName;
            newUser.Email = email;
            newUser.PasswordHash = hashedPassword;
            await unitOfWork.Users.AddNewUser(newUser);
            return await unitOfWork.SaveChangesAsync();
        }
    }
}
