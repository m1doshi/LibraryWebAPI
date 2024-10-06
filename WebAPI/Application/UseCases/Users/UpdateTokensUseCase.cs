using WebAPI.Core.DTOs;
using WebAPI.Core.Interfaces.UnitOfWork;
using WebAPI.Infrastructures.Interfaces;
namespace WebAPI.Application.UseCases.Users
{
    public class UpdateTokensUseCase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IJwtProvider jwtProvider;
        private readonly IRefreshProvider refreshProvider;
        public UpdateTokensUseCase(IUnitOfWork unitOfWork, IJwtProvider jwtProvider, IRefreshProvider refreshProvider)
        {
            this.unitOfWork = unitOfWork;
            this.jwtProvider = jwtProvider;
            this.refreshProvider = refreshProvider;
        }

        public async virtual Task<AuthenticationResponce> UpdateTokens(RefreshTokenRequest tokenRequest)
        {
            var user = await unitOfWork.Users.GetUserByRefreshToken(tokenRequest.RefreshToken);
            if (user == null || user.RefreshTokenExpireTime <= DateTime.UtcNow) 
                throw new Exception("Invalid or expired refresh token.");
            var newJwtToken = jwtProvider.GenerateToken(user);
            var newRefreshToken = refreshProvider.GenerateRefreshToken();
            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpireTime = DateTime.UtcNow.AddDays(7);
            await unitOfWork.Users.UpdateUser(user);
            await unitOfWork.SaveChangesAsync();
            return new AuthenticationResponce 
            { 
                Token = newJwtToken, 
                RefreshToken = newRefreshToken 
            };
        }
    }
}
