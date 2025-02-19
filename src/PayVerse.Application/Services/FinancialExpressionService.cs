using PayVerse.Domain.Interpreters;

namespace PayVerse.Application.Services;

public class FinancialExpressionService
{
    public decimal EvaluateExpression(
        IFinancialExpression expression, 
        FinancialContext context)
    {
        return expression.Interpret(context);
    }

    // Helper method to create a context from current data or external services
    public FinancialContext CreateContext()
    {
        // Here we would typically fetch real-time exchange rates or from a specific date
        var exchangeRates = new Dictionary<string, decimal>
        {
            { "USD", 1.0m },
            { "EUR", 0.85m },
            { "GBP", 0.73m }
        };
        return new FinancialContext(exchangeRates, DateTime.UtcNow);
    }
}