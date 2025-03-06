using PayVerse.Domain.Interpreters.Contexts;

namespace PayVerse.Domain.Interpreters.Expressions;

// Now let's create some specialized expressions for financial operations

// Percentage expression (e.g., for calculating tax or discount)
public class PercentageExpression(
    IFinancialExpression amount, 
    IFinancialExpression percentage) : IFinancialExpression
{
    private readonly IFinancialExpression _amount = amount;
    private readonly IFinancialExpression _percentage = percentage;

    public decimal Interpret(FinancialContext context)
    {
        var amount = _amount.Interpret(context);
        var percentage = _percentage.Interpret(context);

        return amount * (percentage / 100m);
    }
}
