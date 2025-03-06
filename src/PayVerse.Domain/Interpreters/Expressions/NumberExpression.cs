using PayVerse.Domain.Interpreters.Contexts;

namespace PayVerse.Domain.Interpreters.Expressions;

// Terminal Expression for numbers
public class NumberExpression(decimal number) : IFinancialExpression
{
    public decimal Interpret(FinancialContext context)
    {
        return number;
    }
}
