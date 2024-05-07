using CheckDriver.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CheckDricer.Infrastructure.Persistence.Configurations
{
    internal class DoctorEntityConfiguration : IEntityTypeConfiguration<Doctor>
    {
        public void Configure(EntityTypeBuilder<Doctor> builder)
        {
            builder.ToTable(nameof(Doctor));
            builder.HasKey(a => a.Id);

            builder.HasOne(x => x.Account)
                .WithMany(a => a.Doctors)
                .HasForeignKey(x => x.AccountId);

            builder.HasMany(a => a.DoctorReviews)
                .WithOne(x => x.Doctor)
                .HasForeignKey(x => x.DoctorId);
        }
    }
}
