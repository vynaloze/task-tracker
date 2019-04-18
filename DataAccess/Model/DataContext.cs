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
        
        public DbSet<Project> Projects { get; set; }
        
        public DbSet<ToDo> Tasks { get; set; }
        
        public DbSet<Association> Associations { get; set; }
    }
}