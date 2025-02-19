namespace PayVerse.Domain.Interpreters;

// ✅ Allows flexible financial rule interpretation without hardcoding logic.

public interface IFinancialExpression
{
    decimal Interpret(FinancialContext context);
}