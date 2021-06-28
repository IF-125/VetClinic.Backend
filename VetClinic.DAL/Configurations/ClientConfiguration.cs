using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VetClinic.Core.Entities;

namespace VetClinic.DAL.Configurations
{
    class ClientConfiguration : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.ToTable("clients", "vetclinic");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.FirstName)
                    .HasMaxLength(20)
                    .IsRequired();

            builder.Property(c => c.LastName)
                    .HasMaxLength(50)
                    .IsRequired();

            builder.Property(c => c.UserId)
                    .IsRequired();

            builder.HasMany(c => c.PhoneNumbers)
                .WithOne(p => p.Client).HasForeignKey(p => p.ClientId);
        }
    }
}
