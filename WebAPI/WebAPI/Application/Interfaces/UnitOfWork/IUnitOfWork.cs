using WebAPI.Application.Interfaces.Repositories;
using WebAPI.Repositories;

namespace WebAPI.Application.Interfaces.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IBookRepository Books { get; }
        IAuthorRepository Authors { get; }
        IUserRepository Users { get; }
        Task<int> SaveChangesAsync();
    }
}
