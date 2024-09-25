using WebAPI.Domain.Interfaces.Repositories;
using WebAPI.Domain.Interfaces.UnitOfWork;
using WebAPI.Infrastructure.Exceptions;

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
            int result;
            try
            {
                result = await context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                throw new DatabaseOperationException("An error occurred while saving data to the database.", 
                    ex);
            }
            return result;
        }
        public void Dispose()
        {
            context.Dispose();
        }
    }
}
