namespace PayVerse.Domain.Interpreters;

public class AmountExpression(decimal amount) : IFinancialExpression
{
    private readonly decimal _amount = amount;

    public decimal Interpret(FinancialContext context)
    {
        return _amount;
    }
}
