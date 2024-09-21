using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Application.DTOs;
using WebAPI.Application.Interfaces.Services.Users;
using WebAPI.Application.Interfaces.UnitOfWork;
using WebAPI.Infrastructure.Interfaces;
using WebAPI.Infrastructures.Interfaces;
using WebAPI.Infrastructures.Persistence;
namespace WebAPI.Application.UseCases.Users
{
    public class UpdateTokensUseCase : IUpdateTokensService
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

        public async Task<AuthenticationResponce> UpdateTokens(RefreshTokenRequest tokenRequest)
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
