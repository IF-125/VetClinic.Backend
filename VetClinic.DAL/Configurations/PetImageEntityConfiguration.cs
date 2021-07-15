using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VetClinic.Core.Entities;

namespace VetClinic.DAL.Configurations
{
    public class PetImageEntityConfiguration : IEntityTypeConfiguration<PetImage>
    {
        public void Configure(EntityTypeBuilder<PetImage> builder)
        {
            builder
                .HasKey(x => x.Id);
        }
    }
}
