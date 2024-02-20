using Microsoft.EntityFrameworkCore;
using WebTest.Models;


namespace WebTest.Contexts
{

    public class AppDbContext : DbContext
    {
        public DbSet<Person> Persons => Set<Person>();

        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            BuildPersons(modelBuilder);
            BuildSkills(modelBuilder);
        }

        private void BuildSkills(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>().HasKey(x => x.Id);
            modelBuilder.Entity<Person>().Property(x => x.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Person>().HasMany(x => x.Skills).WithOne().HasForeignKey(x=>x.PersonRefKey).IsRequired();
        }

        private void BuildPersons(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Skill>().HasKey(x => x.Id);
            modelBuilder.Entity<Skill>().Property(x => x.Id).ValueGeneratedOnAdd();
        }
    }
    }
