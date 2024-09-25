using WebAPI.Application.DTOs;
using WebAPI.Domain.Exceptions;
using WebAPI.Domain.Interfaces.UnitOfWork;
namespace WebAPI.Application.UseCases.Users
{
    public class UpdateUserUseCase
    {
        private readonly IUnitOfWork unitOfWork;

        public UpdateUserUseCase(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<int> UpdateUser(UserModel updatedUser)
        {
            var user = await unitOfWork.Users.GetUserById(updatedUser.UserID);
            if (user == null)
            {
                throw new EntityNotFoundException("User", updatedUser.UserID);
            }
            await unitOfWork.Users.UpdateUser(updatedUser);
            return await unitOfWork.SaveChangesAsync();
        }
    }
}
