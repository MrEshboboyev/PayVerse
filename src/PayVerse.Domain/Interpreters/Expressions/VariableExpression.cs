using PayVerse.Domain.Interpreters.Contexts;

namespace PayVerse.Domain.Interpreters.Expressions;

// Terminal Expression for variables
public class VariableExpression(string variableName) : IFinancialExpression
{
    public decimal Interpret(FinancialContext context)
    {
        return context.GetVariable(variableName);
    }
}
