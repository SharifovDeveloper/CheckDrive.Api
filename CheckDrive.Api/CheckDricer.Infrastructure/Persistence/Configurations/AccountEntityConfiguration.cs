using CheckDriver.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CheckDricer.Infrastructure.Persistence.Configurations
{
    internal class AccountEntityConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.ToTable(nameof(Account));
            builder.HasKey(x => x.Id);

            builder.Property(a => a.Login)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(a => a.Password)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(a => a.FirstName)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(a => a.LastName)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(a => a.PhoneNumber)
                .HasMaxLength(255)
                .IsRequired();

            builder.HasOne(a => a.Role)
                .WithMany(r => r.Accounts)
                .HasForeignKey(a => a.RoleId);

            builder.HasMany(a => a.Dispatchers)
                .WithOne(x => x.Account)
                .HasForeignKey(x => x.AccountId);

            builder.HasMany(a => a.Operators)
                .WithOne(x => x.Account)
                .HasForeignKey(x => x.AccountId);

            builder.HasMany(a => a.Mechanics)
                .WithOne(x => x.Account)
                .HasForeignKey(x => x.AccountId);

            builder.HasMany(a => a.Drivers)
                .WithOne(x => x.Account)
                .HasForeignKey(x => x.AccountId);

            builder.HasMany(a => a.Doctors)
                .WithOne(x => x.Account)
                .HasForeignKey(x => x.AccountId);

            builder.HasMany(a => a.Technicians)
                .WithOne(x => x.Account)
                .HasForeignKey(x => x.AccountId);
        }
    }
}
