using System.Text;
using PayVerse.Domain.Entities.Invoices;
using PayVerse.Domain.Entities.Notifications;
using PayVerse.Domain.Entities.Payments;
using PayVerse.Domain.Entities.Reports;
using PayVerse.Domain.Entities.Security;
using PayVerse.Domain.Entities.VirtualAccounts;
using PayVerse.Domain.Entities.Wallets;

namespace PayVerse.Domain.Visitors.Implementations;

/// <summary>
/// A visitor that exports entity data to different formats
/// </summary>
public class DataExportVisitor : IVisitor
{
    private readonly StringBuilder _jsonBuilder = new();
    private readonly ExportFormat _format;

    public DataExportVisitor(ExportFormat format)
    {
        _format = format;
        InitializeExport();
    }

    private void InitializeExport()
    {
        if (_format == ExportFormat.Json)
        {
            _jsonBuilder.Append("{\n  \"entities\": [\n");
        }
        // Add initialization for other formats as needed
    }

    public void Visit(Invoice invoice)
    {
        if (_format == ExportFormat.Json)
        {
            _jsonBuilder.Append($"    {{\n      \"type\": \"Invoice\",\n      \"id\": \"{invoice.Id}\",\n");
            _jsonBuilder.Append($"      \"number\": \"{invoice.InvoiceNumber.Value}\",\n");
            _jsonBuilder.Append($"      \"date\": \"{invoice.InvoiceDate.Value:yyyy-MM-dd}\",\n");
            _jsonBuilder.Append($"      \"amount\": {invoice.TotalAmount.Value},\n");
            _jsonBuilder.Append($"      \"status\": \"{invoice.Status}\"\n    }},\n");
        }
        // Add export logic for other formats as needed
    }

    public void Visit(Payment payment)
    {
        if (_format == ExportFormat.Json)
        {
            _jsonBuilder.Append($"    {{\n      \"type\": \"Payment\",\n      \"id\": \"{payment.Id}\",\n");
            _jsonBuilder.Append($"      \"amount\": {payment.Amount.Value},\n");
            _jsonBuilder.Append($"      \"status\": \"{payment.Status}\",\n");
            _jsonBuilder.Append($"      \"date\": \"{payment.ProcessedDate?.ToString("yyyy-MM-dd") ?? "N/A"}\"\n    }},\n");
        }
        // Add export logic for other formats as needed
    }

    public void Visit(VirtualAccount virtualAccount)
    {
        if (_format == ExportFormat.Json)
        {
            _jsonBuilder.Append($"    {{\n      \"type\": \"VirtualAccount\",\n      \"id\": \"{virtualAccount.Id}\",\n");
            _jsonBuilder.Append($"      \"accountNumber\": \"{virtualAccount.AccountNumber.Value}\",\n");
            _jsonBuilder.Append($"      \"balance\": {virtualAccount.Balance.Value},\n");
            _jsonBuilder.Append($"      \"currency\": \"{virtualAccount.Currency.Code}\",\n");
            _jsonBuilder.Append($"      \"status\": \"{virtualAccount.Status}\"\n    }},\n");
        }
        // Add export logic for other formats as needed
    }

    public void Visit(Wallet wallet)
    {
        if (_format == ExportFormat.Json)
        {
            _jsonBuilder.Append($"    {{\n      \"type\": \"Wallet\",\n      \"id\": \"{wallet.Id}\",\n");
            _jsonBuilder.Append($"      \"balance\": {wallet.Balance.Value},\n");
            _jsonBuilder.Append($"      \"currency\": \"{wallet.Currency.Code}\",\n");
            _jsonBuilder.Append($"      \"loyaltyPoints\": {wallet.LoyaltyPoints}\n    }},\n");
        }
        // Add export logic for other formats as needed
    }

    public void Visit(CompositeFinancialReport financialReport)
    {
        if (_format == ExportFormat.Json)
        {
            _jsonBuilder.Append($"    {{\n      \"type\": \"Report\",\n      \"id\": \"{financialReport.Id}\",\n");
            _jsonBuilder.Append($"      \"title\": \"{financialReport.Title.Value}\",\n");
            _jsonBuilder.Append($"      \"type\": \"{financialReport.Type}\",\n");
            _jsonBuilder.Append($"      \"status\": \"{financialReport.Status}\"\n    }},\n");
        }
        // Add export logic for other formats as needed
    }

    public void Visit(Notification notification)
    {
        if (_format == ExportFormat.Json)
        {
            _jsonBuilder.Append($"    {{\n      \"type\": \"Notification\",\n      \"id\": \"{notification.Id}\",\n");
            _jsonBuilder.Append($"      \"message\": \"{notification.Message.Value}\",\n");
            _jsonBuilder.Append($"      \"notificationType\": \"{notification.Type}\",\n");
            _jsonBuilder.Append($"      \"priority\": \"{notification.Priority}\",\n");
            _jsonBuilder.Append($"      \"isRead\": {notification.IsRead.ToString().ToLower()}\n    }},\n");
        }
        // Add export logic for other formats as needed
    }

    public void Visit(SecurityIncident securityIncident)
    {
        if (_format == ExportFormat.Json)
        {
            _jsonBuilder.Append($"    {{\n      \"type\": \"SecurityIncident\",\n      \"id\": \"{securityIncident.Id}\",\n");
            _jsonBuilder.Append($"      \"severity\": \"{securityIncident.Severity}\",\n");
            _jsonBuilder.Append($"      \"status\": \"{securityIncident.Status}\"\n    }},\n");
        }
        // Add export logic for other formats as needed
    }

    public string GetExportData()
    {
        if (_format == ExportFormat.Json)
        {
            // Remove the trailing comma and close the JSON
            string json = _jsonBuilder.ToString().TrimEnd(',', '\n') + "\n  ]\n}";
            return json;
        }

        return "Unsupported format";
    }
}