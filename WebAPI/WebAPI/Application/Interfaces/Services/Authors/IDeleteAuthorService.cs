namespace WebAPI.Application.Interfaces.Services.Authors
{
    public interface IDeleteAuthorService
    {
        Task<int> DeleteAuthor(int authorId);
    }
}
