using Microsoft.EntityFrameworkCore;
using VetClinic.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace VetClinic.DAL.Configurations
{
    public class OrderProcedureEntityConfiguration : IEntityTypeConfiguration<OrderProcedure>
    {
        public void Configure(EntityTypeBuilder<OrderProcedure> builder)
        {
            builder
                .Property(b => b.Conclusion)
                .HasMaxLength(50)
                .IsRequired();

            builder
                .Property(b => b.Pet)
                .IsRequired();

            builder
                .Property(b => b.Employee)
                .IsRequired();

            builder
                .Property(b => b.Procedure)
                .IsRequired();

            builder
                .Property(b => b.Details)
                .HasMaxLength(50);

            builder
                .HasOne(b => b.Pet)
                .WithMany(b => b.OrderProcedures);

            builder
                .HasOne(b => b.Employee)
                .WithMany(b => b.OrderProcedures);

            builder
                .HasOne(b => b.Procedure)
                .WithMany(b => b.OrderProcedures);

            builder
                .HasOne(b => b.Appointment)
                .WithOne(b => b.OrderProcedure)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(b => b.Order)
                .WithOne(b => b.OrderProcedure)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
