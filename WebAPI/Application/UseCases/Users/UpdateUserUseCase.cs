using WebAPI.Core.DTOs;
using WebAPI.Core.Interfaces.UnitOfWork;
using WebAPI.DataAccess.Exceptions;
namespace WebAPI.Application.UseCases.Users
{
    public class UpdateUserUseCase
    {
        private readonly IUnitOfWork unitOfWork;

        public UpdateUserUseCase(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async virtual Task<int> UpdateUser(UserModel updatedUser)
        {
            var user = await unitOfWork.Users.GetUserById(updatedUser.UserID);
            if (user == null) return 0;
            user.UserName = updatedUser.UserName;
            user.Email = updatedUser.Email;
            user.PasswordHash = updatedUser.PasswordHash;
            user.RoleID = updatedUser.RoleID;
            await unitOfWork.Users.UpdateUser(user);
            return await unitOfWork.SaveChangesAsync();
        }
    }
}
