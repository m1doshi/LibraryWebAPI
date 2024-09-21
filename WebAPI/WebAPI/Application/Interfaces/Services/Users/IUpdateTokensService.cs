using WebAPI.Application.DTOs;

namespace WebAPI.Application.Interfaces.Services.Users
{
    public interface IUpdateTokensService
    {
        Task<AuthenticationResponce> UpdateTokens(RefreshTokenRequest tokenRequest);
    }
}
