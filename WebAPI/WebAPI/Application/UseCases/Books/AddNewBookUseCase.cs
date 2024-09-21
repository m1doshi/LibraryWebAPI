using WebAPI.Application.DTOs;
using WebAPI.Application.Interfaces.Services.Book;
using WebAPI.Application.Interfaces.UnitOfWork;


namespace WebAPI.Application.UseCases.Books
{
    public class AddNewBookUseCase : IAddNewBookService
    {
        private readonly IUnitOfWork unitOfWork;
        public AddNewBookUseCase(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<int> AddNewBook(BookModel model)
        {
            await unitOfWork.Books.AddNewBook(model);
            return await unitOfWork.SaveChangesAsync();
        }
    }
}
