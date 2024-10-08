using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Core.DTOs;
using WebAPI.Core.Entities;
using WebAPI.Core.Interfaces.UnitOfWork;
using WebAPI.Infrastructures.Persistence;

namespace Application.UseCases.Users
{
    public class AddNewUserUseCase
    {
        private readonly IUnitOfWork unitOfWork;
        public AddNewUserUseCase(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<int> AddNewUser(UserModel model)
        {
            var user = new User
            {
                UserName = model.UserName,
                Email = model.Email,
                PasswordHash = model.PasswordHash,
            };
            await unitOfWork.Users.AddNewUser(user);
            return await unitOfWork.SaveChangesAsync();
        }
    }
}
