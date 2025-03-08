using PayVerse.Domain.Enums.Security;
using PayVerse.Domain.Repositories.Invoices;
using PayVerse.Domain.Repositories.Notifications;
using PayVerse.Domain.Repositories.Payments;
using PayVerse.Domain.Repositories.Reports;
using PayVerse.Domain.Repositories.VirtualAccounts;
using PayVerse.Domain.Repositories.Wallets;
using PayVerse.Domain.Visitors.Implementations;

namespace PayVerse.Application.Services;

/// <summary>
/// Service for performing operations on multiple types of entities using the Visitor pattern
/// </summary>
public class VisitorOperationService(
    IInvoiceRepository invoiceRepository,
    IPaymentRepository paymentRepository,
    IVirtualAccountRepository virtualAccountRepository,
    IWalletRepository walletRepository,
    ICompositeFinancialReportRepository reportRepository,
    INotificationRepository notificationRepository)
{
    /// <summary>
    /// Generates financial statistics for a user's financial entities
    /// </summary>
    public async Task<FinancialStatisticsSummary> GenerateUserFinancialStatisticsAsync(Guid userId)
    {
        var visitor = new FinancialStatisticsVisitor();

        // Get all user-related entities
        var invoices = await invoiceRepository.GetAllByUserIdAsync(userId);
        var payments = await paymentRepository.GetAllByUserIdAsync(userId);
        var virtualAccounts = await virtualAccountRepository.GetAllByUserIdAsync(userId);
        var wallets = await walletRepository.GetAllByUserIdAsync(userId);
        var reports = await reportRepository.GetByUserIdAsync(userId);
        var notifications = await notificationRepository.GetByUserIdAsync(userId);

        // Apply visitor to each entity
        foreach (var invoice in invoices)
        {
            invoice.Accept(visitor);
        }

        foreach (var payment in payments)
        {
            payment.Accept(visitor);
        }

        foreach (var virtualAccount in virtualAccounts)
        {
            virtualAccount.Accept(visitor);
        }

        foreach (var wallet in wallets)
        {
            wallet.Accept(visitor);
        }

        foreach (var report in reports)
        {
            report.Accept(visitor);
        }

        foreach (var notification in notifications)
        {
            notification.Accept(visitor);
        }

        return visitor.GetStatistics();
    }

    /// <summary>
    /// Exports entity data for a user
    /// </summary>
    public async Task<string> ExportUserDataAsync(Guid userId, ExportFormat format)
    {
        var visitor = new DataExportVisitor(format);

        // Get all user-related entities
        var invoices = await invoiceRepository.GetAllByUserIdAsync(userId);
        var payments = await paymentRepository.GetAllByUserIdAsync(userId);
        var virtualAccounts = await virtualAccountRepository.GetAllByUserIdAsync(userId);
        var wallets = await walletRepository.GetAllByUserIdAsync(userId);
        var reports = await reportRepository.GetByUserIdAsync(userId);
        var notifications = await notificationRepository.GetByUserIdAsync(userId);

        // Apply visitor to each entity
        foreach (var invoice in invoices)
        {
            invoice.Accept(visitor);
        }

        foreach (var payment in payments)
        {
            payment.Accept(visitor);
        }

        foreach (var virtualAccount in virtualAccounts)
        {
            virtualAccount.Accept(visitor);
        }

        foreach (var wallet in wallets)
        {
            wallet.Accept(visitor);
        }

        foreach (var report in reports)
        {
            report.Accept(visitor);
        }

        foreach (var notification in notifications)
        {
            notification.Accept(visitor);
        }

        return visitor.GetExportData();
    }

    /// <summary>
    /// Performs risk assessment on user's financial entities
    /// </summary>
    public async Task<Dictionary<string, List<(Guid Id, RiskLevel Level, string Recommendation)>>> AssessUserRiskAsync(Guid userId)
    {
        var visitor = new RiskAssessmentVisitor();
        var results = new Dictionary<string, List<(Guid Id, RiskLevel Level, string Recommendation)>>();

        // Get all user-related entities
        var invoices = await invoiceRepository.GetAllByUserIdAsync(userId);
        var payments = await paymentRepository.GetAllByUserIdAsync(userId);
        var virtualAccounts = await virtualAccountRepository.GetAllByUserIdAsync(userId);
        var wallets = await walletRepository.GetAllByUserIdAsync(userId);
        var notifications = await notificationRepository.GetByUserIdAsync(userId);

        // Apply visitor to each entity and collect results
        results["Invoices"] = new List<(Guid, RiskLevel, string)>();
        foreach (var invoice in invoices)
        {
            invoice.Accept(visitor);
            results["Invoices"].Add((invoice.Id, visitor.GetRiskLevel(invoice.Id), visitor.GetRecommendation(invoice.Id)));
        }

        results["Payments"] = new List<(Guid, RiskLevel, string)>();
        foreach (var payment in payments)
        {
            payment.Accept(visitor);
            results["Payments"].Add((payment.Id, visitor.GetRiskLevel(payment.Id), visitor.GetRecommendation(payment.Id)));
        }

        results["VirtualAccounts"] = new List<(Guid, RiskLevel, string)>();
        foreach (var virtualAccount in virtualAccounts)
        {
            virtualAccount.Accept(visitor);
            results["VirtualAccounts"].Add((virtualAccount.Id, visitor.GetRiskLevel(virtualAccount.Id), visitor.GetRecommendation(virtualAccount.Id)));
        }

        results["Wallets"] = new List<(Guid, RiskLevel, string)>();
        foreach (var wallet in wallets)
        {
            wallet.Accept(visitor);
            results["Wallets"].Add((wallet.Id, visitor.GetRiskLevel(wallet.Id), visitor.GetRecommendation(wallet.Id)));
        }

        results["Notifications"] = new List<(Guid, RiskLevel, string)>();
        foreach (var notification in notifications)
        {
            notification.Accept(visitor);
            results["Notifications"].Add((notification.Id, visitor.GetRiskLevel(notification.Id), visitor.GetRecommendation(notification.Id)));
        }

        return results;
    }
}