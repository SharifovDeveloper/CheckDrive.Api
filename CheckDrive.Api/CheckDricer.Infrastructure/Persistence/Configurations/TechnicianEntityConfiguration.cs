using CheckDriver.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CheckDricer.Infrastructure.Persistence.Configurations
{
    internal class TechnicianEntityConfiguration : IEntityTypeConfiguration<Technician>
    {
        public void Configure(EntityTypeBuilder<Technician> builder)
        {
            builder.ToTable(nameof(Technician));
            builder.HasKey(t => t.Id);

            builder.HasOne(x => x.Account)
            .WithMany(a => a.Technicians)
            .HasForeignKey(x => x.AccountId);
        }
    }
}
