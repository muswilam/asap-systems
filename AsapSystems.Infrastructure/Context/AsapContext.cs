using AsapSystems.Core.Entities;
using AsapSystems.Core.Enums;
using Microsoft.EntityFrameworkCore;

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
}