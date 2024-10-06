using WebAPI.Core.Interfaces.Repositories;

namespace WebAPI.Core.Interfaces.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IBookRepository Books { get; }
        IAuthorRepository Authors { get; }
        IUserRepository Users { get; }
        Task<int> SaveChangesAsync();
    }
}
