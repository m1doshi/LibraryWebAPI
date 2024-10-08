using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Core.Interfaces.UnitOfWork;

namespace Application.UseCases.Users
{
    public class DeleteUserUseCase
    {
        private readonly IUnitOfWork unitOfWork;
        public DeleteUserUseCase(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<int> DeleteUser(int userId)
        {
            await unitOfWork.Users.DeleteUser(userId);
            return await unitOfWork.SaveChangesAsync();
        }
    }
}
