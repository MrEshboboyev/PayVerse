using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Domain.Entities.Reports;
using PayVerse.Domain.Repositories;
using PayVerse.Domain.Repositories.Invoices;
using PayVerse.Domain.Repositories.Payments;
using PayVerse.Domain.Repositories.Reports;
using PayVerse.Domain.Shared;
using PayVerse.Domain.ValueObjects.Reports;

namespace PayVerse.Application.Reports.Commands.GenerateCompositeFinancialReport;

/// <summary>
/// Handler for the GenerateCompositeFinancialReportCommand
/// </summary>
public class GenerateCompositeFinancialReportCommandHandler(
    ICompositeFinancialReportRepository reportRepository,
    IInvoiceRepository invoiceRepository,
    IPaymentRepository paymentRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<GenerateCompositeFinancialReportCommand, Guid>
{
    public async Task<Result<Guid>> Handle(
        GenerateCompositeFinancialReportCommand request,
        CancellationToken cancellationToken)
    {
        var (userId, title, startDate, endDate, type, fileType) = request;

        // Create a report period
        var period = ReportPeriod.Create(startDate, endDate).Value;

        // Fetch financial data for the specified period
        var invoices = await invoiceRepository.GetInvoicesForPeriodAsync(period.StartDate,
                                                                          period.EndDate,
                                                                          cancellationToken);
        var payments = await paymentRepository.GetPaymentsForPeriodAsync(period.StartDate,
                                                                          period.EndDate,
                                                                          cancellationToken);

        // Build the report using the builder
        var reportBuilder = CompositeFinancialReport.CreateBuilder(
            userId,
            type);

        var titleResult = ReportTitle.Create(title);
        if (titleResult.IsFailure)
        {
            return Result.Failure<Guid>(titleResult.Error);
        }

        // Add income category
        reportBuilder.AddCategory(titleResult.Value.ToString());

        // Group invoices by month
        var invoicesByMonth = GroupInvoicesByMonth(invoices);
        foreach (var (month, monthlyInvoices) in invoicesByMonth)
        {
            reportBuilder.AddCategory(month);

            foreach (var invoice in monthlyInvoices)
            {
                reportBuilder.AddEntry($"Invoice #{invoice.InvoiceNumber.Value}",
                                       invoice.TotalAmount.Value);
            }

            reportBuilder.EndCategory(); // End month category
        }

        reportBuilder.EndCategory(); // End income category

        // Add expenses category
        reportBuilder.AddCategory("Expenses");

        // Group payments by month
        var paymentsByMonth = GroupPaymentsByMonth(payments);
        foreach (var (month, monthlyPayments) in paymentsByMonth)
        {
            reportBuilder.AddCategory(month);

            foreach (var payment in monthlyPayments)
            {
                reportBuilder.AddEntry($"Payment {payment.Id}", payment.Amount.Value);
            }

            reportBuilder.EndCategory(); // End month category
        }

        reportBuilder.EndCategory(); // End expenses category

        // Build the final report
        var report = reportBuilder.Build();

        // Save to repository
        await reportRepository.AddAsync(report, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return report.Id;
    }

    private Dictionary<string, List<Domain.Entities.Invoices.Invoice>> GroupInvoicesByMonth(
        IEnumerable<Domain.Entities.Invoices.Invoice> invoices)
    {
        var result = new Dictionary<string, List<Domain.Entities.Invoices.Invoice>>();

        foreach (var invoice in invoices)
        {
            var month = invoice.InvoiceDate.Value.ToString("MMMM yyyy");

            if (!result.ContainsKey(month))
            {
                result[month] = new List<Domain.Entities.Invoices.Invoice>();
            }

            result[month].Add(invoice);
        }

        return result;
    }

    private Dictionary<string, List<Domain.Entities.Payments.Payment>> GroupPaymentsByMonth(
        IEnumerable<Domain.Entities.Payments.Payment> payments)
    {
        var result = new Dictionary<string, List<Domain.Entities.Payments.Payment>>();

        foreach (var payment in payments)
        {
            var date = payment.ProcessedDate ?? payment.CreatedOnUtc;
            var month = date.ToString("MMMM yyyy");

            if (!result.ContainsKey(month))
            {
                result[month] = new List<Domain.Entities.Payments.Payment>();
            }

            result[month].Add(payment);
        }

        return result;
    }
}
