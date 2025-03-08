using PayVerse.Domain.Entities.Invoices;
using PayVerse.Domain.Entities.Notifications;
using PayVerse.Domain.Entities.Payments;
using PayVerse.Domain.Entities.Reports;
using PayVerse.Domain.Entities.Security;
using PayVerse.Domain.Entities.VirtualAccounts;
using PayVerse.Domain.Entities.Wallets;

namespace PayVerse.Domain.Visitors.Implementations;

/// <summary>
/// A visitor that collects statistical information about financial entities
/// </summary>
public class FinancialStatisticsVisitor : IVisitor
{
    private decimal _totalInvoiceAmount;
    private decimal _totalPaymentAmount;
    private decimal _totalVirtualAccountBalance;
    private decimal _totalWalletBalance;
    private int _totalNotifications;
    private int _totalSecurityIncidents;
    private int _totalReports;

    public void Visit(Invoice invoice)
    {
        _totalInvoiceAmount += invoice.TotalAmount.Value;
    }

    public void Visit(Payment payment)
    {
        _totalPaymentAmount += payment.Amount.Value;
    }

    public void Visit(VirtualAccount virtualAccount)
    {
        _totalVirtualAccountBalance += virtualAccount.Balance.Value;
    }

    public void Visit(Wallet wallet)
    {
        _totalWalletBalance += wallet.Balance.Value;
    }

    public void Visit(CompositeFinancialReport financialReport)
    {
        _totalReports++;
    }

    public void Visit(Notification notification)
    {
        _totalNotifications++;
    }

    public void Visit(SecurityIncident securityIncident)
    {
        _totalSecurityIncidents++;
    }

    public FinancialStatisticsSummary GetStatistics()
    {
        return new FinancialStatisticsSummary(
            _totalInvoiceAmount,
            _totalPaymentAmount,
            _totalVirtualAccountBalance,
            _totalWalletBalance,
            _totalNotifications,
            _totalSecurityIncidents,
            _totalReports);
    }
}

public record FinancialStatisticsSummary(
    decimal TotalInvoiceAmount,
    decimal TotalPaymentAmount,
    decimal TotalVirtualAccountBalance,
    decimal TotalWalletBalance,
    int TotalNotifications,
    int TotalSecurityIncidents,
    int TotalReports);

public enum ExportFormat
{
    Json,
    Xml,
    Csv
}