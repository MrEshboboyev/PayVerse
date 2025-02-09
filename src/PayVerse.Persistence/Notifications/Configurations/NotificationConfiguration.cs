using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PayVerse.Domain.Entities.Notifications;
using PayVerse.Domain.ValueObjects.Notifications;
using PayVerse.Persistence.Notifications.Constants;

namespace PayVerse.Persistence.Notifications.Configurations;

internal sealed class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.ToTable(NotificationTableNames.Notifications);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Message)
            .HasConversion(x => x.Value, v => NotificationMessage.Create(v).Value)
            .IsRequired();

        builder.Property(x => x.Type)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(x => x.Priority)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(x => x.Status)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(x => x.UserId)
            .IsRequired();

        builder.Property(x => x.DeliveryMethod)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(x => x.IsRead)
            .IsRequired();

        builder.Property(x => x.IsSent)
            .IsRequired();

        builder.Property(x => x.SentAt)
            .IsRequired(false);

        builder.Property(x => x.ReadAt)
            .IsRequired(false);

        builder.Property(x => x.CreatedOnUtc)
            .IsRequired();

        builder.Property(x => x.ModifiedOnUtc)
            .IsRequired(false);

        builder.HasIndex(x => x.UserId);
    }
}
