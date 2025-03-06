using PayVerse.Domain.Interpreters.Contexts;

namespace PayVerse.Domain.Interpreters.Expressions;


public interface IFinancialExpression
{
    decimal Interpret(FinancialContext context);
}