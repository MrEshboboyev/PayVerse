using PayVerse.Domain.Entities.Reports;
using PayVerse.Domain.Enums.Reports;
using PayVerse.Domain.ValueObjects.Reports;

namespace PayVerse.Domain.Builders.Reports;

/// <summary>
/// Builder for creating FinancialReport entities
/// </summary>
/// <remarks>
/// Constructor with required parameters
/// </remarks>
public class FinancialReportBuilder(Guid generatedBy,
                                    ReportType reportType) : IBuilder<FinancialReport>
{
    #region Private Properties

    private ReportPeriod _period = ReportPeriod
        .Create(DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(-1)),
                DateOnly.FromDateTime(DateTime.UtcNow)).Value;
    private FileType _fileType = FileType.Pdf;

    #endregion

    #region Building Blocks

    /// <summary>
    /// Sets the report period
    /// </summary>
    public FinancialReportBuilder ForPeriod(DateOnly startDate, DateOnly endDate)
    {
        if (startDate > endDate)
        {
            throw new ArgumentException("Start date must be before end date");
        }

        _period = ReportPeriod.Create(startDate, endDate).Value;
        return this;
    }

    /// <summary>
    /// Sets the report period for a specific month
    /// </summary>
    public FinancialReportBuilder ForMonth(int year, int month)
    {
        var startDate = new DateOnly(year, month, 1);
        var endDate = startDate.AddMonths(1).AddDays(-1);

        _period = ReportPeriod.Create(startDate, endDate).Value;
        return this;
    }

    /// <summary>
    /// Sets the file type
    /// </summary>
    public FinancialReportBuilder AsFileType(FileType fileType)
    {
        _fileType = fileType;
        return this;
    }

    #endregion

    #region Build Construct

    /// <summary>
    /// Builds the FinancialReport instance
    /// </summary>
    public FinancialReport Build()
    {
        return FinancialReport.Create(
            Guid.NewGuid(),
            _period,
            reportType,
            _fileType,
            generatedBy);
    }

    #endregion
}
