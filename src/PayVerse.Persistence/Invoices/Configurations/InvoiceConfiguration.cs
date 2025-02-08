using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PayVerse.Domain.Entities.Invoices;
using PayVerse.Domain.ValueObjects;
using PayVerse.Domain.ValueObjects.Invoices;
using PayVerse.Persistence.Invoices.Constants;

namespace PayVerse.Persistence.Invoices.Configurations;

/// <summary>
/// Configures the Invoice entity for Entity Framework Core.
/// </summary>
internal sealed class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
{
    public void Configure(EntityTypeBuilder<Invoice> builder)
    {
        // Map to the Invoices table
        builder.ToTable(InvoiceTableNames.Invoices);

        // Configure the primary key
        builder.HasKey(x => x.Id);

        // Configure property conversions and constraints
        builder
            .Property(x => x.InvoiceNumber)
            .HasConversion(x => x.Value, v => InvoiceNumber.Create(v).Value)
            .HasMaxLength(InvoiceNumber.MaxLength);

        builder
            .Property(x => x.InvoiceDate)
            .HasConversion(x => x.Value, v => InvoiceDate.Create(v).Value);

        builder
            .Property(x => x.TotalAmount)
            .HasConversion(x => x.Value, v => Amount.Create(v).Value);
        
        builder
            .Property(x => x.Status)
            .HasConversion<string>();

        // Configure RecurringFrequencyInMonths field
        builder
            .Property(x => x.RecurringFrequencyInMonths)
            .IsRequired(false);

        // Configure auditing properties
        builder
            .Property(x => x.CreatedOnUtc)
            .IsRequired();

        builder
            .Property(x => x.ModifiedOnUtc)
            .IsRequired(false);
    }
}