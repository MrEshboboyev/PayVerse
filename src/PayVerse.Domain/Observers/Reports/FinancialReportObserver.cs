using PayVerse.Domain.Entities.Payments;
using PayVerse.Domain.Enums.Payments;
using PayVerse.Domain.Repositories.Reports;
using PayVerse.Domain.ValueObjects.Reports;

namespace PayVerse.Domain.Observers.Reports;


/// <summary>
/// Observer that updates financial reports when payment status changes
/// </summary>
public class FinancialReportObserver(ICompositeFinancialReportRepository reportRepository) : IObserver
{
    public async Task UpdateAsync(ISubject subject)
    {
        if (subject is Payment payment)
        {
            // Only processed and refunded payments affect financial reports
            if (payment.Status == PaymentStatus.Processed ||
                payment.Status == PaymentStatus.Refunded)
            {
                // Find the current month's financial report
                var now = DateTime.UtcNow;
                var startOfMonth = new DateTime(now.Year, now.Month, 1);
                var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

                var reports = await reportRepository.GetByPeriodAsync(
                    ReportPeriod.Create(
                        DateOnly.FromDateTime(startOfMonth), 
                        DateOnly.FromDateTime(endOfMonth)).Value);

                if (reports != null)
                {
                    // Update the report to reflect the payment change
                    // This is a simplified example - real implementation would depend on the report structure
                    await reportRepository.UpdateAsync(reports.First());
                }
            }
        }
    }
}