using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Domain.Events.Reports;
using PayVerse.Domain.Repositories.Reports;

namespace PayVerse.Application.Reports.Events;

internal sealed class ReportGeneratedDomainEventHandler(ICompositeFinancialReportRepository financialReportRepository)
    : IDomainEventHandler<ReportGeneratedDomainEvent>
{
    public async Task Handle(ReportGeneratedDomainEvent notification, CancellationToken cancellationToken)
    {
        var report = await financialReportRepository.GetByIdAsync(
            notification.ReportId, 
            cancellationToken);
        if (report is null)
        {
            return;
        }

        Console.WriteLine($"Report generated with file path: {notification.FilePath}");
    }
}
