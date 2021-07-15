using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VetClinic.Core.Entities;

namespace VetClinic.DAL.Configurations
{
    class AnimalTypeConfiguration : IEntityTypeConfiguration<AnimalType>
    {
        public void Configure(EntityTypeBuilder<AnimalType> builder)
        {
            builder
                .HasKey(x => x.Id);

            builder
                .Property(x => x.Type)
                .IsRequired();

            builder
                .HasMany(x => x.Pets)
                .WithOne()
                .OnDelete(DeleteBehavior.SetNull);

            builder
                .HasMany(x => x.AnimalTypesProcedures)
                .WithOne(x => x.AnimalType)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
