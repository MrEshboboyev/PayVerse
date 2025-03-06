namespace PayVerse.Domain.Interpreters.Contexts;

/// <summary>
/// Context class that contains the information needed for interpretation
/// </summary>
public class FinancialContext
{
    private readonly Dictionary<string, decimal> _variables = [];
    private readonly IReadOnlyDictionary<string, Func<decimal, decimal, decimal>> _operations;

    public FinancialContext()
    {
        _operations = new Dictionary<string, Func<decimal, decimal, decimal>>
        {
            ["+"] = (a, b) => a + b,
            ["-"] = (a, b) => a - b,
            ["*"] = (a, b) => a * b,
            ["/"] = (a, b) => b != 0 
                ? a / b 
                : throw new DivideByZeroException("Cannot divide by zero")
        };
    }

    public void SetVariable(string name, decimal value)
    {
        _variables[name] = value;
    }

    public decimal GetVariable(string name)
    {
        return _variables.TryGetValue(name, out var value)
            ? value
            : throw new KeyNotFoundException($"Variable {name} not found");
    }

    public Func<decimal, decimal, decimal> GetOperation(string operation)
    {
        return _operations.TryGetValue(operation, out var func)
            ? func
            : throw new InvalidOperationException($"Operation {operation} not supported");
    }
}