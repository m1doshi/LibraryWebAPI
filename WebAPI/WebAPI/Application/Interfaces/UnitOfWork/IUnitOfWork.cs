using WebAPI.Application.Interfaces.Repositories;

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
