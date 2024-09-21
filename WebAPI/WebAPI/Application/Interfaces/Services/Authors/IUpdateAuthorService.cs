using WebAPI.Application.DTOs;

namespace WebAPI.Application.Interfaces.Services.Authors
{
    public interface IUpdateAuthorService
    {
        Task<int> UpdateAuthor(int authorId, UpdateAuthorRequest data);
    }
}
