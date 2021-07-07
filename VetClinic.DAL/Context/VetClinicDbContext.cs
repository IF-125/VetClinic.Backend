using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace VetClinic.DAL.Context
{
    public class VetClinicDbContext : DbContext
    { 
        public VetClinicDbContext(DbContextOptions<VetClinicDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
