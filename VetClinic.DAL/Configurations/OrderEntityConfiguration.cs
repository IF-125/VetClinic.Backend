using Microsoft.EntityFrameworkCore;
using VetClinic.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace VetClinic.DAL.Configurations
{
    public class OrderEntityConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder
                .Property(b => b.CreatedTime)
                .IsRequired();
            builder
                .Property(b => b.IsPaid)
                .IsRequired();
        }
    }
}
