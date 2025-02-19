namespace PayVerse.Domain.Interpreters;

public class CurrencyConversionExpression(
    IFinancialExpression amountExpression, 
    string targetCurrency) : IFinancialExpression
{
    private readonly IFinancialExpression _amountExpression = amountExpression;
    private readonly string _targetCurrency = targetCurrency;

    public decimal Interpret(FinancialContext context)
    {
        decimal amount = _amountExpression.Interpret(context);
        return amount * context.GetExchangeRate(_targetCurrency);
    }
}
