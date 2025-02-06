using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PayVerse.Domain.Entities.Wallets;
using PayVerse.Domain.ValueObjects;
using PayVerse.Domain.ValueObjects.Wallets;
using PayVerse.Persistence.Wallets.Constants;

namespace PayVerse.Persistence.Wallets.Configurations;

/// <summary>
/// Configures the Wallet entity for Entity Framework Core.
/// </summary>
internal sealed class WalletConfiguration : IEntityTypeConfiguration<Wallet>
{
    public void Configure(EntityTypeBuilder<Wallet> builder)
    {
        // Map to the Wallets table
        builder.ToTable(WalletTableNames.Wallets);

        // Configure the primary key
        builder.HasKey(x => x.Id);

        // Configure properties
        builder.Property(x => x.Balance)
            .HasConversion(x => x.Value, v => WalletBalance.Create(v).Value)
            .IsRequired();

        builder.Property(x => x.Currency)
            .HasConversion(x => x.Code, v => Currency.Create(v).Value)
            .IsRequired();

        builder.Property(x => x.UserId)
            .IsRequired();

        builder.Property(x => x.CreatedOnUtc)
            .IsRequired();

        builder.Property(x => x.ModifiedOnUtc)
            .IsRequired(false);

        builder.HasMany(x => x.Transactions)
            .WithOne()
            .HasForeignKey("WalletId")
            .OnDelete(DeleteBehavior.Cascade);
    }
}