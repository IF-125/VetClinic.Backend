using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VetClinic.Core.Entities;

namespace VetClinic.DAL.Configurations
{
    public class PositionEntityConfiguration : IEntityTypeConfiguration<Position>
    {
        public void Configure(EntityTypeBuilder<Position> builder)
        {
            builder
                .HasKey(x => x.Id);

            builder
                .Property(x => x.Title)
                .IsRequired();

            builder
                .HasMany(x => x.EmployeePositions)
                .WithOne(x => x.Position)
                .HasForeignKey(x => x.PositionId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
