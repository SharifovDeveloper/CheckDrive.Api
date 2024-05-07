using CheckDriver.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CheckDricer.Infrastructure.Persistence.Configurations
{
    internal class DoctorReviewEntityConfiguration : IEntityTypeConfiguration<DoctorReview>
    {
        public void Configure(EntityTypeBuilder<DoctorReview> builder)
        {
            builder.ToTable(nameof(DoctorReview));

            builder.Property(x => x.IsHealthy)
                .IsRequired();

            builder.Property(x => x.Comments)
                .HasMaxLength(255)
                .IsRequired();

            builder.HasOne(x => x.Driver)
                .WithMany(a => a.DoctorReviews)
                .HasForeignKey(x => x.DriverId);

            builder.HasOne(a => a.Doctor)
                 .WithMany(x => x.DoctorReviews)
                .HasForeignKey(a => a.DoctorId);
        }
    }
}
