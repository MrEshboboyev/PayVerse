using FluentValidation;
using PayVerse.Domain.Entities.Wallets;
using static PayVerse.Domain.Errors.DomainErrors;
using static QuestPDF.Helpers.Colors;

namespace PayVerse.Infrastructure.Proxies;

// ✅ Benefits:
// Ensures only the owner can execute wallet transactions.
// Adds an extra security layer without modifying the core transaction logic.

public interface IWalletTransaction
{
    void Execute(WalletTransaction transaction);
}
