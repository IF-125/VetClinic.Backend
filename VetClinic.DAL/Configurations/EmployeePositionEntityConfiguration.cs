using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VetClinic.Core.Entities;

namespace VetClinic.DAL.Configurations
{
    public class EmployeePositionEntityConfiguration : IEntityTypeConfiguration<EmployeePosition>
    {
        public void Configure(EntityTypeBuilder<EmployeePosition> builder)
        {
            builder.HasKey(x => x.Id);

            builder
                .HasOne(x => x.Employee)
                .WithOne()
                .OnDelete(DeleteBehavior.SetNull);

            builder
                .HasOne(x => x.Position)
                .WithMany(x => x.EmployeePositions);

            builder
                .HasMany(x => x.Salaries)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .Property(x => x.CurrentBaseSalary)
                .IsRequired();

            builder
                .Property(x => x.Rate)
                .IsRequired();
        }
    }
}
