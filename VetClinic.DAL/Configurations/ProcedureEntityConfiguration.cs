using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VetClinic.Core.Entities;

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
                .HasMaxLength(1000);

            builder
                .Property(b => b.Price)
                .IsRequired();
        }
    }
}
