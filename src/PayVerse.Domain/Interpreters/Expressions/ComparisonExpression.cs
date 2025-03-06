using PayVerse.Domain.Interpreters.Contexts;

namespace PayVerse.Domain.Interpreters.Expressions;

// Comparison expression (e.g., for conditional calculations)
public class ComparisonExpression(
    IFinancialExpression left,
    string @operator,
    IFinancialExpression right,
    IFinancialExpression trueResult,
    IFinancialExpression falseResult) : IFinancialExpression
{
    public decimal Interpret(FinancialContext context)
    {
        var leftValue = left.Interpret(context);
        var rightValue = right.Interpret(context);

        bool result = @operator switch
        {
            ">" => leftValue > rightValue,
            "<" => leftValue < rightValue,
            ">=" => leftValue >= rightValue,
            "<=" => leftValue <= rightValue,
            "==" => leftValue == rightValue,
            "!=" => leftValue != rightValue,
            _ => throw new InvalidOperationException($"Operator {@operator} not supported")
        };

        return result ? trueResult.Interpret(context) : falseResult.Interpret(context);
    }
}
