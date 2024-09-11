using Microsoft.EntityFrameworkCore;
using WebAPI.Entities;

namespace WebAPI.Database
{
    public class MyDbContext:DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options):base(options) 
        {
            Database.EnsureCreated();
        }
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors {  get; set; } 
        public DbSet<User> Users { get; set; }
    }
}
