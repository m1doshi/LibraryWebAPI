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
            var book = await unitOfWork.Books.GetBookById(bookId);
            if (book == null) return 0;
            using (var memoryStream = new MemoryStream())
            {
                await image.CopyToAsync(memoryStream);
                var imageData = memoryStream.ToArray();
                await unitOfWork.Books.UpdateImage(bookId, imageData);
            }
            return await unitOfWork.SaveChangesAsync();
        }
    }
}
