using PayVerse.Domain.Entities.VirtualAccounts;
using PayVerse.Domain.Shared;
using PayVerse.Domain.ValueObjects;

namespace PayVerse.Infrastructure.Handlers;

// Chain of Responsibility – Payment Validation Pipeline

// ✅ Future Benefits:
// Can add more handlers dynamically(e.g., KYC verification, transaction risk scoring).
// Reduces code duplication across multiple validation checks.

public interface ITransactionHandler
{
    void SetNext(ITransactionHandler next);
    Result Handle(VirtualAccount account, Amount amount);
}
