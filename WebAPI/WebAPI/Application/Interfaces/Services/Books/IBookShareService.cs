namespace WebAPI.Application.Interfaces.Services.Book
{
    public interface IBookShareService
    {
        Task<bool> IssueBook(int bookId, int userId, DateTime returnDate);
        Task<bool> ReturnBook(int bookId);
    }
}
