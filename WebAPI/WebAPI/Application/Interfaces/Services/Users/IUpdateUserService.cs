using WebAPI.Application.DTOs;

namespace WebAPI.Application.Interfaces.Services.Users
{
    public interface IUpdateUserService
    {
        Task<int> UpdateUser(UserModel updatedUser);
    }
}
