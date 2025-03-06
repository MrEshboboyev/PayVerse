using PayVerse.Domain.Interpreters.Contexts;

namespace PayVerse.Domain.Interpreters.Expressions;

// Function expression (for more complex calculations like MIN, MAX, AVG, etc.)
public class FunctionExpression(
    string functionName, 
    params IFinancialExpression[] parameters) : IFinancialExpression
{
    public decimal Interpret(FinancialContext context)
    {
        var paramValues = parameters.Select(p => p.Interpret(context)).ToArray();

        return functionName.ToUpperInvariant() switch
        {
            "SUM" => paramValues.Sum(),
            "AVG" => paramValues.Any() ? paramValues.Average() : 0,
            "MIN" => paramValues.Any() ? paramValues.Min() : 0,
            "MAX" => paramValues.Any() ? paramValues.Max() : 0,
            "COUNT" => paramValues.Length,
            "ROUND" => paramValues.Length >= 2 ? Math.Round(paramValues[0], (int)paramValues[1]) : Math.Round(paramValues[0]),
            _ => throw new InvalidOperationException($"Function {functionName} not supported")
        };
    }
}