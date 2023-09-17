using Microsoft.EntityFrameworkCore;


namespace WebTest
{

    public class AppContext : DbContext
    {
        private string dbpath = AppDomain.CurrentDomain.BaseDirectory + "itemsDB.sqlite";
        public DbSet<Person> Persons => Set<Person>();

        public AppContext()
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
            modelBuilder.Entity<Person>().HasKey(x => x.id);
            modelBuilder.Entity<Person>().Property(x => x.id).ValueGeneratedOnAdd();
        }

        private void BuildPersons(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Skill>().HasKey(x => x.id);
            modelBuilder.Entity<Skill>().Property(x => x.id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Skill>()
                .HasOne(p => p.person)
                .WithMany(ad => ad.skills)
                .HasForeignKey(ad => ad.personRefKey)
                .IsRequired();
        }
    }
    }
