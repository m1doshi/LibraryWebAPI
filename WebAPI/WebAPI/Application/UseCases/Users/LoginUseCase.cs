using System.Drawing.Text;
using WebAPI.Infrastructure.Interfaces;
using WebAPI.Infrastructures.Interfaces;
using WebAPI.Application.DTOs;
using WebAPI.Domain.Exceptions;
using WebAPI.Domain.Interfaces.UnitOfWork;

namespace WebAPI.Application.UseCases.Users
{
    public class LoginUseCase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IPasswordHasher passwordHasher;
        private readonly IJwtProvider jwtProvider;
        private readonly IRefreshProvider refreshProvider;
        public LoginUseCase(IUnitOfWork unitOfWork, 
            IPasswordHasher passwordHasher, 
            IJwtProvider jwtProvider, 
            IRefreshProvider refreshProvider)
        {
            this.unitOfWork = unitOfWork;
            this.passwordHasher = passwordHasher;
            this.jwtProvider = jwtProvider;
            this.refreshProvider = refreshProvider;
        }
        public async Task<AuthenticationResponce> Login(string email, string password)
        {
            var user = await unitOfWork.Users.GetUserByEmail(email);
            if (user == null)
                throw new EntityNotFoundException("User", email);
            var result = passwordHasher.Verify(password, user.PasswordHash);
            if (result == false) throw new Exception("Failed to login");
            var token = jwtProvider.GenerateToken(user);
            var refreshToken = refreshProvider.GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpireTime = DateTime.UtcNow.AddDays(7);
            await unitOfWork.Users.UpdateUser(user);
            await unitOfWork.SaveChangesAsync();

            return new AuthenticationResponce
            {
                Token = token,
                RefreshToken = refreshToken
            };
        }
    }
}
