using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VetClinic.Core.Entities;

namespace VetClinic.DAL.Configurations
{
    class ClientConfiguration : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.ToTable("Clients", "VetClinic");

            builder.HasMany(c => c.PhoneNumbers)
                .WithOne(p => p.Client).HasForeignKey(p => p.ClientId);
        }
    }
}
