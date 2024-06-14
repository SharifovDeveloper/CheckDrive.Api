using CheckDrive.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CheckDrive.Infrastructure.Persistence.Configurations
{
    internal class DispatcherReviewEntityConfiguration : IEntityTypeConfiguration<DispatcherReview>
    {
        public void Configure(EntityTypeBuilder<DispatcherReview> builder)
        {
            builder.ToTable(nameof(DispatcherReview));
            builder.HasKey(t => t.Id);

            builder.Property(x => x.FuelSpended)
                .IsRequired();

            builder.Property(x => x.DistanceCovered)
                .IsRequired();

            builder.HasOne(d => d.Dispatcher)
                .WithMany(x => x.DispetcherReviews)
                .HasForeignKey(d => d.DispatcherId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(d => d.Operator)
                .WithMany(x => x.DispetcherReviews)
                .HasForeignKey(d => d.OperatorId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(d => d.Mechanic)
                .WithMany(x => x.DispetcherReviews)
                .HasForeignKey(d => d.MechanicId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(d => d.Driver)
                .WithMany(x => x.DispetcherReviews)
                .HasForeignKey(d => d.DriverId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(m => m.Car)
                .WithMany(x => x.Reviewers)
                .HasForeignKey(m => m.CarId)
                .OnDelete(DeleteBehavior.NoAction);
                
        }
    }

}
