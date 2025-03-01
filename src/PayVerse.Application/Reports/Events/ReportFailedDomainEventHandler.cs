using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Domain.Events.Reports;
using PayVerse.Domain.Repositories.Reports;

namespace PayVerse.Application.Reports.Events;

internal sealed class ReportFailedDomainEventHandler(ICompositeFinancialReportRepository financialReportRepository)
    : IDomainEventHandler<ReportFailedDomainEvent>
{
    public async Task Handle(ReportFailedDomainEvent notification, CancellationToken cancellationToken)
    {
        var report = await financialReportRepository.GetByIdAsync(
            notification.ReportId,
            cancellationToken);
        if (report is null)
        {
            return;
        }

        Console.WriteLine($"Report with ID {notification.ReportId} has failed.");
    }
}