using PayVerse.Domain.Entities.Invoices;
using PayVerse.Domain.Entities.Notifications;
using PayVerse.Domain.Entities.Payments;
using PayVerse.Domain.Entities.Reports;
using PayVerse.Domain.Entities.Security;
using PayVerse.Domain.Entities.VirtualAccounts;
using PayVerse.Domain.Entities.Wallets;

namespace PayVerse.Domain.Visitors;

/// <summary>
/// Defines the interface for a visitor that can visit different types of entities in the system.
/// </summary>
public interface IVisitor
{
    void Visit(Invoice invoice);
    void Visit(Payment payment);
    void Visit(VirtualAccount virtualAccount);
    void Visit(Wallet wallet);
    void Visit(CompositeFinancialReport financialReport);
    void Visit(Notification notification);
    void Visit(SecurityIncident securityIncident);
}