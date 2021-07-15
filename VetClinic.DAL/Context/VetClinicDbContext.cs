using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using VetClinic.Core.Entities;
using VetClinic.DAL.Configurations;

namespace VetClinic.DAL.Context
{
    public class VetClinicDbContext : IdentityDbContext<User>
    {
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<AnimalType> AnimalTypes { get; set; }
        public DbSet<AnimalTypeProcedure> AnimalTypeProcedures { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeePosition> EmployeePositions { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderProcedure> OrderProcedures { get; set; }
        public DbSet<Pet> Pets   { get; set; }
        public DbSet<PetImage> PetImages { get; set; }
        public DbSet<PhoneNumber> PhoneNumbers { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Procedure> Procedures { get; set; }
        public DbSet<Salary> Salaries { get; set; }
        public DbSet<Schedule> Schedules { get; set; }

        public VetClinicDbContext(DbContextOptions<VetClinicDbContext> options) : base(options)
        {
        }

      
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
