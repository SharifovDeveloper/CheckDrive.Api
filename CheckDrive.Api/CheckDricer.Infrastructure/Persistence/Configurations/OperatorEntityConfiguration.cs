using CheckDriver.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CheckDricer.Infrastructure.Persistence.Configurations
{
    internal class OperatorEntityConfiguration : IEntityTypeConfiguration<Operator>
    {
        public void Configure(EntityTypeBuilder<Operator> builder)
        {
            builder.ToTable(nameof(Operator));
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Account)
                .WithMany(a => a.Operators)
                .HasForeignKey(x => x.AccountId);

            builder.HasMany(a => a.DispetcherReviews)
               .WithOne(x => x.Operator)
               .HasForeignKey(x => x.OperatorId);

            builder.HasMany(a => a.OperatorReviews)
               .WithOne(x => x.Operator)
               .HasForeignKey(x => x.OperatorId);
        }
    }
}
