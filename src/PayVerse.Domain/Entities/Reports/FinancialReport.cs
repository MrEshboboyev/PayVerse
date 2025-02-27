using PayVerse.Domain.Builders.Reports;
using PayVerse.Domain.Enums.Reports;
using PayVerse.Domain.Errors;
using PayVerse.Domain.Events.Reports;
using PayVerse.Domain.Primitives;
using PayVerse.Domain.Shared;
using PayVerse.Domain.ValueObjects.Reports;

namespace PayVerse.Domain.Entities.Reports;

public sealed class FinancialReport : AggregateRoot, IAuditableEntity
{
    #region Constructors
    
    private FinancialReport(
        Guid id,
        ReportPeriod period,
        ReportType type,
        FileType fileType, 
        Guid generatedBy)
        : base(id)
    {
        Period = period;
        Type = type;
        FileType = fileType;
        GeneratedBy = generatedBy;
        GeneratedAt = DateTime.UtcNow;
        Status = ReportStatus.Pending;
    }
    
    #endregion
    
    #region Properties

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

    public static FinancialReport Create(
        Guid id,
        ReportPeriod period,
        ReportType type,
        FileType fileType,
        Guid userId)
    {
        return new FinancialReport(id, period, type, fileType, userId);
    }
    
    #endregion
    
    #region Own Methods

    public Result MarkAsCompleted(string filePath)
    {
        if (Status != ReportStatus.Pending)
        {
            return Result.Failure(
                DomainErrors.FinancialReport.CannotMarkAsCompleted(Id));
        }
        
        Status = ReportStatus.Completed;
        FilePath = filePath;
        
        RaiseDomainEvent(new ReportGeneratedDomainEvent(
            Guid.NewGuid(),
            Id,
            FilePath));
        
        return Result.Success();
    }

    public Result MarkAsFailed()
    {
        if (Status != ReportStatus.Pending)
        {
            return Result.Failure(
                DomainErrors.FinancialReport.CannotMarkAsFailed(Id));
        }
        Status = ReportStatus.Failed;
        
        RaiseDomainEvent(new ReportFailedDomainEvent(
            Guid.NewGuid(),
            Id));
        
        return Result.Success();
    }

    #endregion

    #region Builders

    // Factory method for the builder
    public static FinancialReportBuilder CreateBuilder(Guid generatedBy,
                                                       ReportType reportType)
    {
        return new FinancialReportBuilder(generatedBy, reportType);
    }

    #endregion
}