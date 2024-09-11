using WebAPI.Repositories;
using WebAPI.Repositories.Interfaces;

namespace WebAPI.UnitOfWork
{
    public interface IUnitOfWork:IDisposable
    {
        IBookRepository Books { get; }
        IAuthorRepository Authors { get; }
        IUserRepository Users { get; }
        Task<int> SaveChangesAsync();
    }
}
