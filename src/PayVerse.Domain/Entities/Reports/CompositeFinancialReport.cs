using PayVerse.Domain.Builders.Reports;
using PayVerse.Domain.Composites.Reports;
using PayVerse.Domain.Enums.Reports;
using PayVerse.Domain.Errors;
using PayVerse.Domain.Events.Reports;
using PayVerse.Domain.Primitives;
using PayVerse.Domain.Shared;
using PayVerse.Domain.ValueObjects.Reports;
using PayVerse.Domain.Visitors;
using PayVerse.Domain.Visitors.Reports;

namespace PayVerse.Domain.Entities.Reports;

public sealed class CompositeFinancialReport : AggregateRoot, IAuditableEntity, IVisitable
{
    #region Private fields

    private IReportComponent _rootComponent;

    #endregion

    #region Constructors

    private CompositeFinancialReport(
        Guid id,
        ReportTitle title,
        ReportPeriod period,
        ReportType type,
        FileType fileType, 
        Guid generatedBy)
        : base(id)
    {
        Title = title;
        Period = period;
        Type = type;
        FileType = fileType;
        GeneratedBy = generatedBy;
        GeneratedAt = DateTime.UtcNow;
        Status = ReportStatus.Pending;
    }
    
    #endregion
    
    #region Properties

    public ReportTitle Title { get; private set; }
    public ReportPeriod Period { get; private set; }
    public ReportType Type { get; private set; }
    public FileType FileType { get; private set; }
    public ReportStatus Status { get; private set; }
    public string FilePath { get; private set; }
    public Guid GeneratedBy { get; private set; }
    public DateTime GeneratedAt { get; private set; }
    public DateTime CreatedOnUtc { get; set; }
    public DateTime? ModifiedOnUtc { get; set; }
    
    #endregion
    
    #region Factory Methods

    public static CompositeFinancialReport Create(
        Guid id,
        ReportTitle title,
        ReportPeriod period,
        ReportType type,
        FileType fileType,
        Guid userId)
    {
        return new CompositeFinancialReport(id, title, period, type, fileType, userId);
    }

    #endregion

    #region Own Methods

    #region Composite related

    public IReportComponent GetReportStructure() => _rootComponent;

    public Result SetReportStructure(IReportComponent rootComponent)
    {
        _rootComponent = rootComponent;

        return Result.Success();
    }

    public decimal GetTotalAmount() => _rootComponent.GetTotal();

    public List<string> GenerateSummary()
    {
        var visitor = new ReportSummaryVisitor();
        _rootComponent.Accept(visitor);
        return visitor.GetSummaries() as List<string>;
    }

    #endregion

    public Result MarkAsCompleted(string filePath)
    {
        if (Status != ReportStatus.Pending)
        {
            return Result.Failure(
                DomainErrors.CompositeFinancialReport.CannotMarkAsCompleted(Id));
        }
        
        Status = ReportStatus.Completed;
        FilePath = filePath;
        
        RaiseDomainEvent(new ReportGeneratedDomainEvent(
            Guid.NewGuid(),
            Id,
            FilePath));
        
        return Result.Success();
    }

    public Result MarkAsFailed(string reason)
    {
        if (Status != ReportStatus.Pending)
        {
            return Result.Failure(
                DomainErrors.CompositeFinancialReport.CannotMarkAsFailed(Id));
        }
        Status = ReportStatus.Failed;
        
        RaiseDomainEvent(new ReportFailedDomainEvent(
            Guid.NewGuid(),
            Id,
            reason,
            DateTime.UtcNow));
        
        return Result.Success();
    }

    #endregion

    #region Builders

    // Factory method for the builder
    public static CompositeFinancialReportBuilder CreateBuilder(Guid generatedBy,
                                                       ReportType reportType)
    {
        return new CompositeFinancialReportBuilder(generatedBy, reportType);
    }

    #endregion

    #region Visitor Pattern Implementation

    public void Accept(IVisitor visitor)
    {
        visitor.Visit(this);
    }

    #endregion
}