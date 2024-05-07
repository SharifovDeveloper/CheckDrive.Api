using CheckDriver.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CheckDricer.Infrastructure.Persistence.Configurations
{
    internal class DispatcherReviewEntityConfiguration : IEntityTypeConfiguration<DispatcherReview>
    {
        public void Configure(EntityTypeBuilder<DispatcherReview> builder)
        {
            builder.ToTable(nameof(DispatcherReview));
            builder.HasKey(t => t.Id);

            builder.Property(x => x.FuelSpended)
                .HasColumnType("double")
                .IsRequired();

            builder.Property(x => x.DistanceCovered)
                .HasColumnType("double")
                .IsRequired();

            builder.HasOne(d => d.Dispatcher)
                .WithMany(x => x.DispetcherReviews)
                .HasForeignKey(d => d.DispatcherId);

            builder.HasOne(d => d.Operator)
                .WithMany(x => x.DispetcherReviews)
                .HasForeignKey(d => d.OperatorId);

            builder.HasOne(d => d.Mechanic)
                .WithMany(x => x.DispetcherReviews)
                .HasForeignKey(d => d.MechanicId);

            builder.HasOne(d => d.Driver)
                .WithMany(x => x.DispetcherReviews)
                .HasForeignKey(d => d.DriverId);
        }
    }
}
