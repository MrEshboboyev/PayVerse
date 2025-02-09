namespace PayVerse.Domain.Events.Reports;

public sealed record ReportGeneratedDomainEvent(
    Guid Id, 
    Guid ReportId,
    string FilePath) : DomainEvent(Id);