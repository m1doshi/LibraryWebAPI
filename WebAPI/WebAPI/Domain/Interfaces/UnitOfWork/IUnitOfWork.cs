using WebAPI.Domain.Interfaces.Repositories;

namespace WebAPI.Domain.Interfaces.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IBookRepository Books { get; }
        IAuthorRepository Authors { get; }
        IUserRepository Users { get; }
        Task<int> SaveChangesAsync();
    }
}
