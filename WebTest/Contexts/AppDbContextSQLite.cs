using Microsoft.EntityFrameworkCore;

namespace WebTest.Contexts
{
    public class AppDbContextSQLite : AppDbContext
    {
        public AppDbContextSQLite(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }
}
