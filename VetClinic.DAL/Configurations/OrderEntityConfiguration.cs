using Microsoft.EntityFrameworkCore;
using VetClinic.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace VetClinic.DAL.Configurations
{
    public class OrderEntityConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder
                .Property(b => b.CreatedAt)
                .IsRequired();

            builder
                .Property(b => b.PaymentOption)
                .IsRequired();

            builder
                .HasOne(b => b.OrderProcedure)
                .WithOne(b => b.Order)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
