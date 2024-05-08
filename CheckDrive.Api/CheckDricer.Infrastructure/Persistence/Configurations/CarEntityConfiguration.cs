using CheckDriver.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CheckDricer.Infrastructure.Persistence.Configurations
{
    internal class CarEntityConfiguration : IEntityTypeConfiguration<Car>
    {
        public void Configure(EntityTypeBuilder<Car> builder)
        {
            builder.ToTable("Car");
            builder.HasKey(c => c.Id);

            builder.Property(x => x.Model)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(x => x.Color)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(x => x.Number)
                .HasMaxLength(8)
                .IsRequired();

            builder.Property(x => x.ManufacturedYear)
                .IsRequired();

            builder.Property(x => x.MeduimFuelConsumption)
                .IsRequired();

            builder.Property(x => x.FuelTankCapacity)
                .IsRequired();

            builder.HasMany(c => c.MechanicHandovers)
                .WithOne(m => m.Car)
                .HasForeignKey(m => m.CarId);
        }
    }
}
