using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VetClinic.Core.Entities;

namespace VetClinic.DAL.Configurations
{
    public class ScheduleEntityConfiguration : IEntityTypeConfiguration<Schedule>
    {
        public void Configure(EntityTypeBuilder<Schedule> builder)
        {
            builder
                .HasKey(x => x.Id);

            builder
                .Property(x => x.From)
                .IsRequired();

            builder
                .Property(x => x.To)
                .IsRequired();

            builder
                .Property(x => x.Day)
                .IsRequired();
        }
    }
}
