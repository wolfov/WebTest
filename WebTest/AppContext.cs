using Microsoft.EntityFrameworkCore;
using WebTest.Models;


namespace WebTest
{

    public class AppContext : DbContext
    {
        private string dbpath = AppDomain.CurrentDomain.BaseDirectory + "itemsDB.sqlite";
        public DbSet<Person> Persons => Set<Person>();

        public AppContext(DbContextOptions<AppContext> options): base(options)
        {
            Database.Migrate();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={dbpath}");
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
        }

        private void BuildPersons(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Skill>().HasKey(x => x.Id);
            modelBuilder.Entity<Skill>().Property(x => x.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Skill>()
                .HasOne(p => p.Person)
                .WithMany(ad => ad.Skills)
                .HasForeignKey(ad => ad.PersonRefKey)
                .IsRequired();
        }
    }
    }
