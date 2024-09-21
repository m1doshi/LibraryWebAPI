namespace WebAPI.Application.Interfaces.Services.Book
{
    public interface IUpdateImageService
    {
        Task<int> UpdateImage(int bookId, IFormFile image);
    }
}
