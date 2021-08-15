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
                .Property(x => x.Address)
                .IsRequired();

            builder
                .HasOne(x => x.EmployeePosition)
                .WithOne(x => x.Employee)
                .OnDelete(DeleteBehavior.Cascade);

            //builder
            //    .HasMany(o => o.OrderProcedures)
            //    .WithOne(e => e.Employee)
            //    .HasForeignKey(x => x.EmployeeId)
            //    .OnDelete(DeleteBehavior.SetNull);

            builder
               .HasMany(x => x.Schedule)
               .WithOne(x => x.Employee)
               .HasForeignKey(x => x.EmployeeId)
               .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
