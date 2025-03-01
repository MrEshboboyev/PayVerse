using PayVerse.Domain.Composites.Reports;
using PayVerse.Domain.Entities.Reports;
using PayVerse.Domain.Enums.Reports;
using PayVerse.Domain.ValueObjects.Reports;

namespace PayVerse.Domain.Builders.Reports;

/// <summary>
/// Builder for creating CompositeFinancialReport entities
/// </summary>
public class CompositeFinancialReportBuilder : IBuilder<CompositeFinancialReport>
{
    #region Private Properties

    private ReportTitle _title = ReportTitle.Create("Financial Report").Value;
    private ReportPeriod _period;
    private readonly ReportType _reportType;
    private FileType _fileType = FileType.Pdf;
    private readonly Guid _generatedBy;
    private FinancialCategory _currentCategory;
    private readonly Stack<FinancialCategory> _categoryStack = new();
    private FinancialCategory _rootCategory;

    #endregion

    #region Constructor

    /// <summary>
    /// Constructor with required parameters
    /// </summary>
    public CompositeFinancialReportBuilder(Guid generatedBy,
                                           ReportType reportType)
    {
        _generatedBy = generatedBy;
        _reportType = reportType;
        _period = ReportPeriod
            .Create(DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(-1)),
                    DateOnly.FromDateTime(DateTime.UtcNow)).Value;

        // Initialize the root category
        _rootCategory = new FinancialCategory(_title.ToString());
        _currentCategory = _rootCategory;
    }

    #endregion

    #region Building Blocks

    /// <summary>
    /// Sets the report title
    /// </summary>
    public CompositeFinancialReportBuilder WithTitle(ReportTitle title)
    {
        _title = title;
        // Update root category name
        _rootCategory = new FinancialCategory(title.ToString());
        _currentCategory = _rootCategory;
        _categoryStack.Clear();
        return this;
    }

    /// <summary>
    /// Sets the report period
    /// </summary>
    public CompositeFinancialReportBuilder ForPeriod(DateOnly startDate, DateOnly endDate)
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
    public CompositeFinancialReportBuilder ForMonth(int year, int month)
    {
        var startDate = new DateOnly(year, month, 1);
        var endDate = startDate.AddMonths(1).AddDays(-1);
        _period = ReportPeriod.Create(startDate, endDate).Value;
        return this;
    }

    /// <summary>
    /// Sets the file type
    /// </summary>
    public CompositeFinancialReportBuilder AsFileType(FileType fileType)
    {
        _fileType = fileType;
        return this;
    }

    /// <summary>
    /// Adds a new category to the report structure
    /// </summary>
    public CompositeFinancialReportBuilder AddCategory(string name)
    {
        var newCategory = new FinancialCategory(name);
        _currentCategory.Add(newCategory);
        _categoryStack.Push(_currentCategory);
        _currentCategory = newCategory;
        return this;
    }

    /// <summary>
    /// Adds a financial entry to the current category
    /// </summary>
    public CompositeFinancialReportBuilder AddEntry(string name, decimal amount)
    {
        _currentCategory.Add(new FinancialEntry(name, amount));
        return this;
    }

    /// <summary>
    /// Returns to the parent category
    /// </summary>
    public CompositeFinancialReportBuilder EndCategory()
    {
        if (_categoryStack.Count > 0)
        {
            _currentCategory = _categoryStack.Pop();
        }
        return this;
    }

    #endregion

    #region Build Construct

    /// <summary>
    /// Builds the CompositeFinancialReport instance
    /// </summary>
    public CompositeFinancialReport Build()
    {
        // Ensure we're back at the root level
        while (_categoryStack.Count > 0)
        {
            _currentCategory = _categoryStack.Pop();
        }

        // Create the report
        var report = CompositeFinancialReport.Create(
            Guid.NewGuid(),
            _title,
            _period,
            _reportType,
            _fileType,
            _generatedBy);

        // Set the report structure
        report.SetReportStructure(_rootCategory);

        return report;
    }

    #endregion
}