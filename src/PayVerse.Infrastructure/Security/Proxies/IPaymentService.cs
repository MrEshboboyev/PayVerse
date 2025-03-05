// Abstract interface for payment operations
namespace PayVerse.Infrastructure.Security.Proxies;

public interface IPaymentService
{
    Task<bool> ProcessPaymentAsync(Guid userId, decimal amount);
    Task<decimal> GetAccountBalanceAsync(Guid userId);
}
