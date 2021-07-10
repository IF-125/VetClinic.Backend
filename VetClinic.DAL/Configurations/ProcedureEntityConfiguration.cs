using Microsoft.EntityFrameworkCore;
using VetClinic.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace VetClinic.DAL.Configurations
{
    public class ProcedureEntityConfiguration : IEntityTypeConfiguration<Procedure>
    {
        public void Configure(EntityTypeBuilder<Procedure> builder)
        {
            builder
                .Property(b => b.Title)
                .HasMaxLength(50)
                .IsRequired();

            builder
                .Property(b => b.Description)
                .HasMaxLength(50);

            builder
                .Property(b => b.Price)
                .IsRequired();

            builder
                .HasMany(x => x.OrderProcedures)
                .WithOne()
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
