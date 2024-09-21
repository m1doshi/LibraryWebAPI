using WebAPI.Application.Interfaces.Repositories;
using WebAPI.Application.Interfaces.UnitOfWork;

namespace WebAPI.Infrastructures.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MyDbContext context;
        public IUserRepository Users { get; private set; }
        public IBookRepository Books { get; private set; }
        public IAuthorRepository Authors { get; private set; }

        public UnitOfWork(MyDbContext context,
            IUserRepository userRepository,
            IBookRepository bookRepository,
            IAuthorRepository authorRepository)
        {
            this.context = context;
            Users = userRepository;
            Books = bookRepository;
            Authors = authorRepository;
        }
        public async Task<int> SaveChangesAsync()
        {
            return await context.SaveChangesAsync();
        }
        public void Dispose()
        {
            context.Dispose();
        }
    }
}
