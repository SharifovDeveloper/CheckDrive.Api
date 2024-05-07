using CheckDriver.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CheckDricer.Infrastructure.Persistence.Configurations
{
    internal class OperatorReviewEntityConfiguration : IEntityTypeConfiguration<OperatorReview>
    {
        public void Configure(EntityTypeBuilder<OperatorReview> builder)
        {
            builder.ToTable(nameof(OperatorReview));

            builder.Property(x => x.OilAmount)
                .HasColumnType("double")
                .IsRequired();

            builder.Property(x => x.Comments)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(x => x.Status)
                .HasMaxLength(255)
                .IsRequired();

            builder.HasOne(o => o.Operator)
                .WithMany(x => x.OperatorReviews)
                .HasForeignKey(o => o.OperatorId);

            builder.HasOne(o => o.Driver)
               .WithMany(x => x.OperatorReviews)
               .HasForeignKey(o => o.DriverId);
        }
    }
}
