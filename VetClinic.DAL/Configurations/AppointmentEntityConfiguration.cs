using Microsoft.EntityFrameworkCore;
using VetClinic.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace VetClinic.DAL.Configurations
{
    public class AppointmentEntityConfiguration : IEntityTypeConfiguration<Appointment>
    {
        public void Configure(EntityTypeBuilder<Appointment> builder)
        {
            builder
                .Property(b => b.Status)
                .IsRequired();

            builder
                .Property(b => b.From)
                .IsRequired();

            builder
                .Property(b => b.To)
                .IsRequired();

            builder
                .HasOne(b => b.OrderProcedure)
                .WithOne(b => b.Appointment)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
