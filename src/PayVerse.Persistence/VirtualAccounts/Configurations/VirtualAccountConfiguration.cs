using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PayVerse.Domain.Entities.VirtualAccounts;
using PayVerse.Domain.ValueObjects;
using PayVerse.Domain.ValueObjects.VirtualAccounts;
using PayVerse.Persistence.VirtualAccounts.Constants;

namespace PayVerse.Persistence.VirtualAccounts.Configurations;

/// <summary>
/// Configures the VirtualAccount entity for Entity Framework Core.
/// </summary>
internal sealed class VirtualAccountConfiguration : IEntityTypeConfiguration<VirtualAccount>
{
    public void Configure(EntityTypeBuilder<VirtualAccount> builder)
    {
        // Map to the VirtualAccounts table
        builder.ToTable(VirtualAccountTableNames.VirtualAccounts);

        // Configure the primary key
        builder.HasKey(x => x.Id);

        // Configure properties
        builder.Property(x => x.AccountNumber)
            .HasConversion(x => x.Value, v => AccountNumber.Create(v).Value)
            .IsRequired();

        builder.Property(x => x.Currency)
            .HasConversion(x => x.Code, v => Currency.Create(v).Value)
            .IsRequired();

        builder.Property(x => x.Balance)
            .HasConversion(x => x.Value, v => Balance.Create(v).Value)
            .IsRequired();

        builder.Property(x => x.UserId).IsRequired();

        builder.HasMany(x => x.Transactions)
            .WithOne()
            .HasForeignKey("VirtualAccountId")
            .OnDelete(DeleteBehavior.Cascade);
    }
}