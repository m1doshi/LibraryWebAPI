namespace WebAPI.Application.Interfaces.Services.Book
{
    public interface IDeleteBookService
    {
        Task<int> DeleteBook(int bookId);
    }
}
