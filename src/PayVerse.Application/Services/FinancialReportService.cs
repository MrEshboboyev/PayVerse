using PayVerse.Domain.Composites.Reports;
using PayVerse.Domain.Entities.Reports;
using PayVerse.Domain.Enums.Invoices;
using PayVerse.Domain.Enums.Payments;
using PayVerse.Domain.Interpreters.Contexts;
using PayVerse.Domain.Interpreters.Parsers;
using PayVerse.Domain.Repositories.Invoices;
using PayVerse.Domain.Repositories.Payments;
using PayVerse.Domain.Repositories.Reports;
using PayVerse.Domain.Repositories.VirtualAccounts;
using PayVerse.Domain.Repositories.Wallets;

namespace PayVerse.Application.Services;

/// <summary>
/// Domain service for generating financial reports using the Interpreter pattern
/// </summary>
public class FinancialReportService(
    ICompositeFinancialReportRepository reportRepository,
    IPaymentRepository paymentRepository,
    IInvoiceRepository invoiceRepository,
    IVirtualAccountRepository virtualAccountRepository,
    IWalletRepository walletRepository)
{
    /// <summary>
    /// Generates a report based on the specified report data and expression
    /// </summary>
    public async Task<CompositeFinancialReport> GenerateReportAsync(
        CompositeFinancialReport report,
        IEnumerable<string> expressions)
    {
        var context = await CreateFinancialContextAsync(report);
        var results = new Dictionary<string, decimal>();

        foreach (var expr in expressions)
        {
            try
            {
                var parser = new FinancialExpressionParser(expr);
                var expression = parser.Parse();
                var expressionResult = expression.Interpret(context);

                // Store the result with a key based on the expression
                results[expr] = expressionResult;
            }
            catch (Exception ex)
            {
                // Log error and continue with other expressions
                // In a real application, you might want to handle this differently
                results[$"ERROR_{expr}"] = 0;
            }
        }

        // Create report sections based on results
        await CreateReportSectionsAsync(report, results);

        // Update report status - assuming there's a file path after processing
        var filePath = GenerateReportFilePath(report);
        var result = report.MarkAsCompleted(filePath);

        if (result.IsFailure)
        {
            // Handle the failure case
            report.MarkAsFailed("Failed to mark report as completed");
        }

        await reportRepository.UpdateAsync(report);

        return report;
    }

    /// <summary>
    /// Creates and populates the financial context with relevant data for the report period
    /// </summary>
    private async Task<FinancialContext> CreateFinancialContextAsync(CompositeFinancialReport report)
    {
        var context = new FinancialContext();
        var startDate = report.Period.StartDate;
        var endDate = report.Period.EndDate;

        // Populate data from Payments
        var payments = await paymentRepository.GetPaymentsForPeriodAsync(startDate, endDate);
        context.SetVariable("total_payments", payments.Sum(p => p.Amount.Value));
        context.SetVariable("successful_payments",
            payments.Where(p => p.Status == PaymentStatus.Processed).Sum(p => p.Amount.Value));
        context.SetVariable("failed_payments",
            payments.Where(p => p.Status == PaymentStatus.Failed).Sum(p => p.Amount.Value));
        context.SetVariable("refunded_payments",
            payments.Where(p => p.Status == PaymentStatus.Refunded).Sum(p => p.Amount.Value));
        context.SetVariable("payment_count", payments.Count());

        // Populate data from Invoices
        var invoices = await invoiceRepository.GetInvoicesForPeriodAsync(startDate, endDate);
        context.SetVariable("total_invoices", invoices.Sum(i => i.TotalAmount.Value));
        context.SetVariable("paid_invoices",
            invoices.Where(i => i.Status == InvoiceStatus.Paid).Sum(i => i.TotalAmount.Value));
        context.SetVariable("unpaid_invoices",
            invoices.Where(i => i.Status == InvoiceStatus.Sent || i.Status == InvoiceStatus.Overdue)
            .Sum(i => i.TotalAmount.Value));
        context.SetVariable("invoice_count", invoices.Count());

        // Populate data from VirtualAccounts
        var accounts = await virtualAccountRepository.GetAllActiveAccountsAsync();
        context.SetVariable("total_balance", accounts.Sum(a => a.Balance.Value));
        context.SetVariable("account_count", accounts.Count());

        // Populate data from Wallets
        var wallets = await walletRepository.GetAllAsync();
        context.SetVariable("total_wallet_balance", wallets.Sum(w => w.Balance.Value));
        context.SetVariable("wallet_count", wallets.Count());

        // Calculate derived metrics
        if (context.GetVariable("invoice_count") > 0)
        {
            context.SetVariable("average_invoice_value",
                context.GetVariable("total_invoices") / context.GetVariable("invoice_count"));
        }

        if (context.GetVariable("payment_count") > 0)
        {
            context.SetVariable("average_payment_value",
                context.GetVariable("total_payments") / context.GetVariable("payment_count"));
        }

        return context;
    }

    /// <summary>
    /// Creates sections in the composite report based on calculation results
    /// </summary>
    private async Task CreateReportSectionsAsync(
        CompositeFinancialReport report,
        Dictionary<string, decimal> results)
    {
        // Create the root financial category
        var rootCategory = new FinancialCategory("Financial Results");

        // Create categories for grouping results if needed
        var metricsCategory = new FinancialCategory("Key Metrics");
        var invoiceCategory = new FinancialCategory("Invoice Data");
        var paymentCategory = new FinancialCategory("Payment Data");

        // Add entries to appropriate categories
        foreach (var result in results)
        {
            var entry = new FinancialEntry(result.Key, result.Value);

            // You could implement logic to categorize entries based on their key names
            if (result.Key.Contains("invoice"))
            {
                invoiceCategory.Add(entry);
            }
            else if (result.Key.Contains("payment"))
            {
                paymentCategory.Add(entry);
            }
            else
            {
                metricsCategory.Add(entry);
            }
        }

        // Add subcategories to the root category
        rootCategory.Add(metricsCategory);
        rootCategory.Add(invoiceCategory);
        rootCategory.Add(paymentCategory);

        // Set the root component in the report
        var setResult = report.SetReportStructure(rootCategory);

        if (setResult.IsFailure)
        {
            // Handle the failure case
            await Task.CompletedTask; // Just to maintain the async signature
            throw new InvalidOperationException("Failed to set report structure");
        }
    }

    /// <summary>
    /// Generates a file path for the report
    /// </summary>
    private string GenerateReportFilePath(CompositeFinancialReport report)
    {
        // Implement logic to generate a file path based on report properties
        var fileExtension = report.FileType.ToString().ToLower();
        var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
        var fileName = $"{report.Title.Value}_{timestamp}.{fileExtension}";

        // You might want to use a configuration setting for the base path
        return Path.Combine("reports", fileName);
    }
}