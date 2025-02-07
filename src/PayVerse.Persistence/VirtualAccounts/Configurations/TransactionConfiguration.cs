using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PayVerse.Domain.Entities.VirtualAccounts;
using PayVerse.Domain.ValueObjects;
using PayVerse.Persistence.VirtualAccounts.Constants;

namespace PayVerse.Persistence.VirtualAccounts.Configurations;

/// <summary>
/// Configures the Transaction entity for Entity Framework Core.
/// </summary>
internal sealed class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        // Map to the Transactions table
        builder.ToTable(VirtualAccountTableNames.Transactions);

        // Configure the primary key
        builder.HasKey(x => x.Id);
        
        // Explicitly map InvoiceId as a non-nullable foreign key
        builder
            .Property(x => x.VirtualAccountId)
            .IsRequired();

        builder
            .HasOne<VirtualAccount>()
            .WithMany(x => x.Transactions)
            .HasForeignKey(x => x.VirtualAccountId)
            .OnDelete(DeleteBehavior.Cascade); // Cascade delete

        // Configure properties
        builder.Property(x => x.Amount)
            .HasConversion(x => x.Value, v => Amount.Create(v).Value)
            .IsRequired();

        builder.Property(x => x.Date).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(500);
    }
}