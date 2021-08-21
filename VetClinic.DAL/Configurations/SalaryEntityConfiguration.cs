using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using VetClinic.Core.Entities;

namespace VetClinic.DAL.Configurations
{
    public class SalaryEntityConfiguration : IEntityTypeConfiguration<Salary>
    {
        public void Configure(EntityTypeBuilder<Salary> builder)
        {
            builder
                .HasKey(x => x.Id);

            builder
                .Property(x => x.Amount)
                .IsRequired();

            builder
                .Property(x => x.Bonus)
                .IsRequired();

            builder
                .Property(x => x.Date)
                .IsRequired();

            builder
                .HasOne(x => x.EmployeePosition)
                .WithMany(x => x.Salaries);
        }
    }
}
