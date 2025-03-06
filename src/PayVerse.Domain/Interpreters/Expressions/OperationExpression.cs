using PayVerse.Domain.Interpreters.Contexts;

namespace PayVerse.Domain.Interpreters.Expressions;

// Non-terminal Expression for operations
public class OperationExpression(
    string operation, 
    IFinancialExpression left, 
    IFinancialExpression right) : IFinancialExpression
{
    private readonly string _operation = operation;

    public decimal Interpret(FinancialContext context)
    {
        var leftResult = left.Interpret(context);
        var rightResult = right.Interpret(context);
        var operation = context.GetOperation(_operation);

        return operation(leftResult, rightResult);
    }
}
