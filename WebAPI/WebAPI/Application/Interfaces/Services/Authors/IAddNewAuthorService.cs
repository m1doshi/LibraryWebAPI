using WebAPI.Application.DTOs;

namespace WebAPI.Application.Interfaces.Services.Authors
{
    public interface IAddNewAuthorService
    {
        Task<int> AddNewAuthor(AuthorModel author);
    }
}
