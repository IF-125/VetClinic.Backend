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
                .Property(b => b.Begin)
                .IsRequired();
            builder
                .Property(b => b.End)
                .IsRequired();
        }
    }
}
