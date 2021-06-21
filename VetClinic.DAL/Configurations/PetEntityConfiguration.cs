using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace VetClinic.DAL.Configurations
{
    class PetEntityConfiguration: IEntityTypeConfiguration<Pet>
    {
        public void Configure(EntityTypeBuilder<Pet> builder)
        {
            builder
                .HasKey(x => x.Id);

            builder
                .HasOne(x => x.AnimalType)
                .WithMany(x => x.Pets)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder
                .HasOne(x => x.Pet)
                .WithMany(x => x.PetImages)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
