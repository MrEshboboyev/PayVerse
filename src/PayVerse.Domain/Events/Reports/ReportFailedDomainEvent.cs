namespace PayVerse.Domain.Events.Reports;

public sealed record ReportFailedDomainEvent(
    Guid Id,
    Guid ReportId) : DomainEvent(Id);