namespace PayVerse.Domain.Events.Reports;

public sealed record ReportFailedDomainEvent(
    Guid Id,
    Guid ReportId,
    string Reason,
    DateTime OccurredOnUtc) : DomainEvent(Id);