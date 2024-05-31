using CheckDrive.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CheckDrive.Infrastructure.Persistence.Configurations
{
    internal class OperatorReviewEntityConfiguration : IEntityTypeConfiguration<OperatorReview>
    {
        public void Configure(EntityTypeBuilder<OperatorReview> builder)
        {
            builder.ToTable(nameof(OperatorReview));
            builder.HasKey(x => x.Id);

            builder.Property(x => x.OilAmount)
                .IsRequired();

            builder.Property(x => x.Comments)
                .HasMaxLength(255);

            builder.Property(x => x.Status)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(x => x.OilMarks)
                .HasMaxLength(255)
                .IsRequired();

            builder.HasOne(o => o.Operator)
                .WithMany(x => x.OperatorReviews)
                .HasForeignKey(o => o.OperatorId)
                .OnDelete(DeleteBehavior.NoAction); 

            builder.HasOne(o => o.Driver)
                .WithMany(x => x.OperatorReviews)
                .HasForeignKey(o => o.DriverId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(o => o.Car)
                .WithMany(x => x.OperatorReviews)
                .HasForeignKey(o => o.CarId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }

}
