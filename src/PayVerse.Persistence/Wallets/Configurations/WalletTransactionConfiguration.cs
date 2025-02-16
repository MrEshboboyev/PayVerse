using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PayVerse.Domain.Entities.Wallets;
using PayVerse.Domain.ValueObjects;
using PayVerse.Persistence.Wallets.Constants;

namespace PayVerse.Persistence.Wallets.Configurations;

/// <summary>
/// Configures the WalletTransaction entity for Entity Framework Core.
/// </summary>
internal sealed class WalletTransactionConfiguration : IEntityTypeConfiguration<WalletTransaction>
{
    public void Configure(EntityTypeBuilder<WalletTransaction> builder)
    {
        // Map to the WalletTransactions table
        builder.ToTable(WalletTableNames.WalletTransactions);

        // Configure the primary key
        builder.HasKey(x => x.Id);
        
        // Explicitly map WalletId as a non-nullable foreign key
        builder
            .Property(x => x.WalletId)
            .IsRequired();

        builder
            .HasOne<Wallet>()
            .WithMany(x => x.Transactions)
            .HasForeignKey(x => x.WalletId)
            .OnDelete(DeleteBehavior.Cascade); // Cascade delete

        // Configure properties
        builder.Property(x => x.Amount)
            .HasConversion(x => x.Value, v => Amount.Create(v).Value)
            .IsRequired();

        builder.Property(x => x.Date)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasMaxLength(500);
    }
}