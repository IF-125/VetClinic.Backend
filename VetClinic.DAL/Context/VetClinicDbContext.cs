using Microsoft.EntityFrameworkCore;
using System.Reflection;
using VetClinic.Core.Entities;

namespace VetClinic.DAL.Context
{
    public class VetClinicDbContext : DbContext
    { 
        public VetClinicDbContext(DbContextOptions<VetClinicDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Pet> Pets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
