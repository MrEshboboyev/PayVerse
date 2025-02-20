using PayVerse.Domain.Entities.Invoices;
using PayVerse.Domain.Visitors;
using Microsoft.Extensions.Logging;

namespace PayVerse.Application.Visitors;

public class AuditVisitor(ILogger<AuditVisitor> logger) : IInvoiceVisitor
{
    private readonly ILogger<AuditVisitor> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public void Visit(Invoice invoice)
    {
        _logger.LogInformation($"🔍 Auditing Invoice {invoice.Id}: " +
            $"Total Amount={invoice.TotalAmount.Value}, " +
            $"Status={invoice.Status}, " +
            $"Date={invoice.InvoiceDate}");
        // Here you could store audit details in an audit log or trigger a domain event
    }
}