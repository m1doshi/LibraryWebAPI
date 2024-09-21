using WebAPI.Application.DTOs;
using WebAPI.Application.Interfaces.Services.Book;
using WebAPI.Application.Interfaces.UnitOfWork;

namespace WebAPI.Application.UseCases.Books
{
    public class UpdateImageUseCase : IUpdateImageService
    {
        private readonly IUnitOfWork unitOfWork;
        public UpdateImageUseCase(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<int> UpdateImage(int bookId, IFormFile image)
        {
            await unitOfWork.Books.UpdateImage(bookId, image);
            return await unitOfWork.SaveChangesAsync();
        }
    }
}
