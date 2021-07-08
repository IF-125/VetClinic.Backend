using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VetClinic.Core.Entities;

namespace VetClinic.DAL.Configurations
{
    public class EmployeeEntityConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder
                .HasKey(x => x.Id);

            builder
                .HasOne(x => x.EmployeePosition)
                .WithOne(x => x.Employee)
                .OnDelete(DeleteBehavior.Cascade);

            builder
               .HasMany(x => x.Schedules)
               .WithOne()
               .OnDelete(DeleteBehavior.Cascade);

            builder
                .Property(x => x.Address)
                .IsRequired();
        }
    }
}
