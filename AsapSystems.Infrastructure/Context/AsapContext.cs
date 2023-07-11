using AsapSystems.Core.Entities;
using AsapSystems.Core.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace AsapSystems.Infrastructure.Context
{
    public class AsapContext : DbContext
    {
        public AsapContext(DbContextOptions options)
            : base(options)
        {
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var genders = Enum.GetValues(typeof(GenderEnum))
                            .Cast<GenderEnum>()
                            .Select(ge => new Gender { Id = (int)ge, Name = ge.ToString() });

            modelBuilder.Entity<Gender>().HasData(genders);
        }

        public DbSet<Person> Persons { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<Address> Addresses { get; set; }
    }

    public class AsapFactory : IDesignTimeDbContextFactory<AsapContext>
    {
        public AsapFactory()
        {
            
        }

        public AsapContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AsapContext>();
            optionsBuilder.UseSqlServer("Data source=.;initial catalog=AsapDB;Integrated security=True;MultipleActiveResultSets=True;");

            return new AsapContext(optionsBuilder.Options);
        }
    }
}