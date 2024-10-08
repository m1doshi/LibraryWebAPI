using WebAPI.Core.DTOs;
using WebAPI.Core.Interfaces.UnitOfWork;

namespace WebAPI.Application.UseCases.Authors
{
    public class UpdateAuthorUseCase
    {
        private readonly IUnitOfWork unitOfWork;
        public UpdateAuthorUseCase(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async virtual Task<int> UpdateAuthor(int authorId, UpdateAuthorRequest data)
        {
            var author = await unitOfWork.Authors.GetAuthorById(authorId);
            if (author == null) return 0;
            author.FirstName = data.FirstName;
            author.LastName = data.LastName;
            author.Birthday = data.Birthday;
            author.Country = data.Country;
            await unitOfWork.Authors.UpdateAuthor(author);
            return await unitOfWork.SaveChangesAsync();
        }
    }
}
