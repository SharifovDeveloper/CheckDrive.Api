using CheckDrive.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CheckDrive.Infrastructure.Persistence.Configurations
{
    internal class DispatcherEntityConfiguration : IEntityTypeConfiguration<Dispatcher>
    {
        public void Configure(EntityTypeBuilder<Dispatcher> builder)
        {
            builder.ToTable(nameof(Dispatcher));
            builder.HasKey(t => t.Id);


            builder.HasOne(x => x.Account)
                .WithMany(a => a.Dispatchers)
                .HasForeignKey(x => x.AccountId);

            builder.HasMany(a => a.DispetcherReviews)
                .WithOne(x => x.Dispatcher)
                .HasForeignKey(x => x.DispatcherId);
        }
    }
}
