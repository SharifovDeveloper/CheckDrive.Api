using CheckDrive.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CheckDrive.Infrastructure.Persistence.Configurations
{
    internal class UndeliveredMessageConfiguration : IEntityTypeConfiguration<UndeliveredMessage>
    {
        public void Configure(EntityTypeBuilder<UndeliveredMessage> builder)
        {
            builder.ToTable(nameof(UndeliveredMessage));
            builder.HasKey(x => x.Id);

            builder.Property(x => x.ReviewId)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(x => x.UserId)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(x => x.Message)
                .HasMaxLength(500)
                .IsRequired();
        }
    }
}
