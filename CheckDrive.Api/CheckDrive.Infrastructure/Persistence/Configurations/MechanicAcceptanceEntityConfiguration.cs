using CheckDrive.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CheckDrive.Infrastructure.Persistence.Configurations
{
    internal class MechanicAcceptanceEntityConfiguration : IEntityTypeConfiguration<MechanicAcceptance>
    {
        public void Configure(EntityTypeBuilder<MechanicAcceptance> builder)
        {
            builder.ToTable(nameof(MechanicAcceptance));
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Comments)
                .HasMaxLength(255);

            builder.Property(x => x.Status)
                .HasMaxLength(255)
                .IsRequired();

            builder.HasOne(m => m.Mechanic)
                .WithMany(x => x.MechanicAcceptance)
                .HasForeignKey(m => m.MechanicId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(m => m.Car)
                .WithMany(x => x.MechanicAcceptance)
                .HasForeignKey(m => m.CarId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(m => m.Driver)
                .WithMany(x => x.MechanicAcceptance)
                .HasForeignKey(m => m.DriverId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Property(x => x.Distance)
            .IsRequired();

        }
    }
}
