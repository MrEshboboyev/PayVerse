using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PayVerse.Domain.Entities.Reports;
using PayVerse.Persistence.Reports.Constants;

namespace PayVerse.Persistence.Reports.Configurations;

internal sealed class CompositeFinancialReportConfiguration : IEntityTypeConfiguration<CompositeFinancialReport>
{
    public void Configure(EntityTypeBuilder<CompositeFinancialReport> builder)
    {
        builder.ToTable(ReportTableNames.CompositeFinancialReports);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Period)
            .HasConversion(new ReportPeriodConverter())
            .IsRequired();

        builder.Property(x => x.Type)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(x => x.Status)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(x => x.FilePath)
            .IsRequired(false);

        builder.Property(x => x.GeneratedBy)
            .IsRequired();

        builder.Property(x => x.GeneratedAt)
            .IsRequired();

        builder.Property(x => x.CreatedOnUtc)
            .IsRequired();

        builder.Property(x => x.ModifiedOnUtc)
            .IsRequired(false);

        builder.HasIndex(x => x.GeneratedBy);
    }
}
