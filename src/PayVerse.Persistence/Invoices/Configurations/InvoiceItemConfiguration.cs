using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PayVerse.Domain.Entities.Invoices;
using PayVerse.Domain.ValueObjects;
using PayVerse.Persistence.Invoices.Constants;

namespace PayVerse.Persistence.Invoices.Configurations;

/// <summary>
/// Configures the InvoiceItem entity for Entity Framework Core.
/// </summary>
internal sealed class InvoiceItemConfiguration : IEntityTypeConfiguration<InvoiceItem>
{
    public void Configure(EntityTypeBuilder<InvoiceItem> builder)
    {
        // Map to the InvoiceItems table
        builder.ToTable(InvoiceTableNames.InvoiceItems);

        // Configure the primary key
        builder.HasKey(x => x.Id);
        
        // Explicitly map InvoiceId as a non-nullable foreign key
        builder
            .Property(x => x.InvoiceId)
            .IsRequired();

        builder
            .HasOne<Invoice>()
            .WithMany(x => x.Items)
            .HasForeignKey(x => x.InvoiceId)
            .OnDelete(DeleteBehavior.Cascade); // Cascade delete

        // Configure property conversions and constraints
        builder
            .Property(x => x.Description)
            .HasMaxLength(255)
            .IsRequired();

        builder
            .Property(x => x.Amount)
            .HasConversion(x => x.Value, v => Amount.Create(v).Value)
            .IsRequired();
    }
}