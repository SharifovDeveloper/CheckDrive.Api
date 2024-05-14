using CheckDrive.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CheckDrive.Infrastructure.Persistence.Configurations
{
    internal class DriverEntityConfiguration : IEntityTypeConfiguration<Driver>
    {
        public void Configure(EntityTypeBuilder<Driver> builder)
        {
            builder.ToTable(nameof(Driver));
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Account)
                .WithMany(a => a.Drivers)
                .HasForeignKey(x => x.AccountId);

            builder.HasMany(a => a.DispetcherReviews)
                .WithOne(x => x.Driver)
                .HasForeignKey(x => x.DriverId);

            builder.HasMany(a => a.DoctorReviews)
                .WithOne(x => x.Driver)
                .HasForeignKey(x => x.DriverId);

            builder.HasMany(a => a.MechanicHandovers)
                .WithOne(x => x.Driver)
                .HasForeignKey(x => x.DriverId);

            builder.HasMany(a => a.OperatorReviews)
                .WithOne(x => x.Driver)
                .HasForeignKey(x => x.DriverId);
        }
    }
}
