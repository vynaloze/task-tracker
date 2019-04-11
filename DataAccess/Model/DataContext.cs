using Microsoft.EntityFrameworkCore;

namespace DataAccess.Model
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}