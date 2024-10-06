using Microsoft.AspNetCore.Http;
using WebAPI.Core.Interfaces.UnitOfWork;

namespace WebAPI.Application.UseCases.Books
{
    public class UpdateImageUseCase
    {
        private readonly IUnitOfWork unitOfWork;
        public UpdateImageUseCase(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async virtual Task<int> UpdateImage(int bookId, IFormFile image)
        {
            var book = unitOfWork.Books.GetBookById(bookId);
            if (book!=null && image != null && image.Length > 0)
            {
                await unitOfWork.Books.UpdateImage(bookId, image);
                return await unitOfWork.SaveChangesAsync();
            }
            return 0;
        }
    }
}
