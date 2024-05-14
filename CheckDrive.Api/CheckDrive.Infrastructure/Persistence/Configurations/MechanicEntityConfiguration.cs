using CheckDrive.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CheckDrive.Infrastructure.Persistence.Configurations
{
    internal class MechanicEntityConfiguration : IEntityTypeConfiguration<Mechanic>
    {
        public void Configure(EntityTypeBuilder<Mechanic> builder)
        {
            builder.ToTable(nameof(Mechanic));
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Account)
                .WithMany(a => a.Mechanics)
                .HasForeignKey(x => x.AccountId);

            builder.HasMany(a => a.DispetcherReviews)
                .WithOne(x => x.Mechanic)
                .HasForeignKey(x => x.MechanicId);

            builder.HasMany(a => a.MechanicHandovers)
                .WithOne(x => x.Mechanic)
                .HasForeignKey(x => x.MechanicId);
        }
    }
}
