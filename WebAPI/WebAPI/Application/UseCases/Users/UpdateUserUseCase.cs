using WebAPI.Application.DTOs;
using WebAPI.Application.Interfaces;
using WebAPI.Application.Interfaces.Services.Users;
using WebAPI.Application.Interfaces.UnitOfWork;
namespace WebAPI.Application.UseCases.Users
{
    public class UpdateUserUseCase : IUpdateUserService
    {
        private readonly IUnitOfWork unitOfWork;

        public UpdateUserUseCase(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<int> UpdateUser(UserModel updatedUser)
        {
            await unitOfWork.Users.UpdateUser(updatedUser);
            return await unitOfWork.SaveChangesAsync();
        }
    }
}
