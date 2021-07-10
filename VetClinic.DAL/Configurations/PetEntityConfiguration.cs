using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VetClinic.Core.Entities;


namespace VetClinic.DAL.Configurations
{
    class PetEntityConfiguration : IEntityTypeConfiguration<Pet>
    {
        public void Configure(EntityTypeBuilder<Pet> builder)
        {
            builder
                .HasKey(x => x.Id);
                        
            builder
                .HasOne(x => x.AnimalType)
                .WithMany(x=>x.Pets)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasMany(x=>x.PetImages)
                .WithOne(x=>x.Pet)
                .OnDelete(DeleteBehavior.Cascade);

            

        }
    }
}
