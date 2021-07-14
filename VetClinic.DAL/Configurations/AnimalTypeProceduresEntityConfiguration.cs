using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VetClinic.Core.Entities;

namespace VetClinic.DAL.Configurations
{
    public class AnimalTypeProceduresEntityConfiguration : IEntityTypeConfiguration<AnimalTypeProcedure>
    {
        public void Configure(EntityTypeBuilder<AnimalTypeProcedure> builder)
        {
            builder
                .HasKey(ap => new { ap.ProcedureId, ap.AnimalTypeId });

            builder
                .HasOne(ap => ap.Procedure)
                .WithMany(a => a.AnimalTypesProcedures)
                .HasForeignKey(x => x.ProcedureId);

            builder
                .HasOne(ap => ap.AnimalType)
                .WithMany(a => a.AnimalTypesProcedures)
                .HasForeignKey(a => a.AnimalTypeId);
        }
    }
}
