using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PayVerse.Domain.Entities.Payments;
using PayVerse.Domain.Enums.Payments;
using PayVerse.Domain.ValueObjects.Payments;
using PayVerse.Persistence.Payments.Constants;

namespace PayVerse.Persistence.Payments.Configurations;

/// <summary>
/// Configures the Payment entity for Entity Framework Core.
/// </summary>
internal sealed class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        // Map to the Payments table
        builder.ToTable(PaymentTableNames.Payments);

        // Configure the primary key
        builder.HasKey(x => x.Id);

        // Configure properties
        builder.Property(x => x.Amount)
            .HasConversion(x => x.Value, v => PaymentAmount.Create(v).Value)
            .IsRequired();

        builder.Property(x => x.Status)
            .HasConversion(
                x => x.ToString(),
                v => Enum.Parse<PaymentStatus>(v))
            .IsRequired();

        builder.Property(x => x.UserId)
            .IsRequired();

        builder.Property(x => x.CreatedOnUtc)
            .IsRequired();

        builder.Property(x => x.ModifiedOnUtc)
            .IsRequired(false);
        
        builder.Property(x => x.ScheduledDate)
            .IsRequired(false);
    }
}