using PayVerse.Domain.Entities.Invoices;
using PayVerse.Domain.Entities.Notifications;
using PayVerse.Domain.Entities.Payments;
using PayVerse.Domain.Entities.Reports;
using PayVerse.Domain.Entities.Security;
using PayVerse.Domain.Entities.VirtualAccounts;
using PayVerse.Domain.Entities.Wallets;
using PayVerse.Domain.Enums.Invoices;
using PayVerse.Domain.Enums.Notifications;
using PayVerse.Domain.Enums.Payments;
using PayVerse.Domain.Enums.Security;

namespace PayVerse.Domain.Visitors.Implementations;

/// <summary>
/// A visitor that performs risk assessment on financial entities
/// </summary>
public class RiskAssessmentVisitor : IVisitor
{
    private readonly Dictionary<Guid, RiskLevel> _entityRiskLevels = new();
    private readonly Dictionary<Guid, string> _riskRecommendations = new();

    public void Visit(Invoice invoice)
    {
        var riskLevel = RiskLevel.Low;
        var recommendation = "No action needed.";

        // Example risk assessment logic
        if (invoice.TotalAmount.Value > 10000)
        {
            riskLevel = RiskLevel.Medium;
            recommendation = "Review high-value invoice details.";
        }

        if (invoice.Status == InvoiceStatus.Overdue)
        {
            riskLevel = RiskLevel.High;
            recommendation = "Immediate follow-up required for overdue invoice.";
        }

        _entityRiskLevels[invoice.Id] = riskLevel;
        _riskRecommendations[invoice.Id] = recommendation;
    }

    public void Visit(Payment payment)
    {
        var riskLevel = RiskLevel.Low;
        var recommendation = "No action needed.";

        // Example risk assessment logic
        if (payment.Amount.Value > 10000)
        {
            riskLevel = RiskLevel.Medium;
            recommendation = "Review large payment for compliance.";
        }

        if (payment.Status == PaymentStatus.Failed)
        {
            riskLevel = RiskLevel.High;
            recommendation = "Investigate payment failure.";
        }

        _entityRiskLevels[payment.Id] = riskLevel;
        _riskRecommendations[payment.Id] = recommendation;
    }

    public void Visit(VirtualAccount virtualAccount)
    {
        var riskLevel = RiskLevel.Low;
        var recommendation = "No action needed.";

        // Example risk assessment logic
        if (virtualAccount.Balance.Value < 0)
        {
            riskLevel = RiskLevel.Medium;
            recommendation = "Account has negative balance, monitor activity.";
        }

        if (virtualAccount.Transactions.Count > 100)
        {
            riskLevel = RiskLevel.Medium;
            recommendation = "High transaction volume account, review for unusual patterns.";
        }

        _entityRiskLevels[virtualAccount.Id] = riskLevel;
        _riskRecommendations[virtualAccount.Id] = recommendation;
    }

    public void Visit(Wallet wallet)
    {
        var riskLevel = RiskLevel.Low;
        var recommendation = "No action needed.";

        // Example risk assessment logic
        if (wallet.SpendingLimit.HasValue && wallet.Balance.Value > wallet.SpendingLimit.Value * 0.9m)
        {
            riskLevel = RiskLevel.Medium;
            recommendation = "Wallet approaching spending limit, notify user.";
        }

        _entityRiskLevels[wallet.Id] = riskLevel;
        _riskRecommendations[wallet.Id] = recommendation;
    }

    public void Visit(CompositeFinancialReport financialReport)
    {
        var riskLevel = RiskLevel.Low;
        var recommendation = "No action needed.";

        _entityRiskLevels[financialReport.Id] = riskLevel;
        _riskRecommendations[financialReport.Id] = recommendation;
    }

    public void Visit(Notification notification)
    {
        var riskLevel = RiskLevel.Low;
        var recommendation = "No action needed.";

        // Example risk assessment logic
        if (notification.Priority == NotificationPriority.High && !notification.IsRead)
        {
            riskLevel = RiskLevel.Medium;
            recommendation = "High priority notification unread, follow up required.";
        }

        _entityRiskLevels[notification.Id] = riskLevel;
        _riskRecommendations[notification.Id] = recommendation;
    }

    public void Visit(SecurityIncident securityIncident)
    {
        var riskLevel = RiskLevel.Medium;
        var recommendation = "Review security incident.";

        // Example risk assessment logic
        if (securityIncident.Severity == SecurityIncidentSeverity.Critical)
        {
            riskLevel = RiskLevel.Critical;
            recommendation = "Critical security incident requires immediate attention.";
        }

        _entityRiskLevels[securityIncident.Id] = riskLevel;
        _riskRecommendations[securityIncident.Id] = recommendation;
    }

    public RiskLevel GetRiskLevel(Guid entityId)
    {
        return _entityRiskLevels.GetValueOrDefault(entityId, RiskLevel.Unknown);
    }

    public string GetRecommendation(Guid entityId)
    {
        return _riskRecommendations.TryGetValue(entityId, out string recommendation) ? recommendation : "No assessment available.";
    }

    public IReadOnlyDictionary<Guid, RiskLevel> GetAllRiskLevels() => _entityRiskLevels.AsReadOnly();

    public IReadOnlyDictionary<Guid, string> GetAllRecommendations() => _riskRecommendations.AsReadOnly();
}