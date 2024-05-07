using CheckDriver.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CheckDricer.Infrastructure.Persistence.Configurations
{
    internal class MechanicAcceptanceEntityConfiguration : IEntityTypeConfiguration<MechanicAcceptance>
    {
        public void Configure(EntityTypeBuilder<MechanicAcceptance> builder)
        {
            builder.ToTable(nameof(MechanicAcceptance));
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Comments)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(x => x.Status)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(x => x.Distance)
                .IsRequired();

            builder.HasOne(m => m.MechanicHandover)
                .WithMany(x => x.MechanicAcceptances)
                .HasForeignKey(m => m.MechanicHandoverId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
