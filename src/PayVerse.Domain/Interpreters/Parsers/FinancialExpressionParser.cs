using PayVerse.Domain.Interpreters.Expressions;

namespace PayVerse.Domain.Interpreters.Parsers;

// Parser for the financial expression language
public class FinancialExpressionParser(string expression)
{
    private readonly string _expression = expression.Replace(" ", "");
    private int _position = 0;

    public IFinancialExpression Parse()
    {
        return ParseExpression();
    }

    private IFinancialExpression ParseExpression()
    {
        var left = ParseTerm();

        while (_position < _expression.Length)
        {
            var currentChar = _expression[_position];

            if (currentChar == '+' || currentChar == '-')
            {
                _position++;
                var right = ParseTerm();
                left = new OperationExpression(currentChar.ToString(), left, right);
            }
            else if (currentChar == ')' || currentChar == ',')
            {
                break;
            }
            else
            {
                throw new InvalidOperationException($"Unexpected character: {currentChar}");
            }
        }

        return left;
    }

    private IFinancialExpression ParseTerm()
    {
        var left = ParseFactor();

        while (_position < _expression.Length)
        {
            var currentChar = _expression[_position];

            if (currentChar == '*' || currentChar == '/')
            {
                _position++;
                var right = ParseFactor();
                left = new OperationExpression(currentChar.ToString(), left, right);
            }
            else
            {
                break;
            }
        }

        return left;
    }

    private IFinancialExpression ParseFactor()
    {
        if (_position >= _expression.Length)
        {
            throw new InvalidOperationException("Unexpected end of expression");
        }

        var currentChar = _expression[_position];

        // Parse number
        if (char.IsDigit(currentChar) || currentChar == '.')
        {
            return ParseNumber();
        }
        // Parse function
        else if (char.IsLetter(currentChar))
        {
            var startPos = _position;

            // Read function name or variable
            while (_position < _expression.Length &&
                  (char.IsLetterOrDigit(_expression[_position]) || _expression[_position] == '_'))
            {
                _position++;
            }

            var nameOrVar = _expression.Substring(startPos, _position - startPos);

            // Check if it's a function
            if (_position < _expression.Length && _expression[_position] == '(')
            {
                _position++; // Skip '('

                var parameters = new List<IFinancialExpression>();

                if (_expression[_position] != ')')
                {
                    parameters.Add(ParseExpression());

                    while (_position < _expression.Length && _expression[_position] == ',')
                    {
                        _position++; // Skip ','
                        parameters.Add(ParseExpression());
                    }
                }

                if (_position >= _expression.Length || _expression[_position] != ')')
                {
                    throw new InvalidOperationException("Missing closing parenthesis");
                }

                _position++; // Skip ')'

                return new FunctionExpression(nameOrVar, parameters.ToArray());
            }
            // It's a variable
            else
            {
                return new VariableExpression(nameOrVar);
            }
        }
        // Parse parenthesized expression
        else if (currentChar == '(')
        {
            _position++; // Skip '('
            var result = ParseExpression();

            if (_position >= _expression.Length || _expression[_position] != ')')
            {
                throw new InvalidOperationException("Missing closing parenthesis");
            }

            _position++; // Skip ')'
            return result;
        }
        else
        {
            throw new InvalidOperationException($"Unexpected character: {currentChar}");
        }
    }

    private IFinancialExpression ParseNumber()
    {
        var startPos = _position;

        // Parse integer part
        while (_position < _expression.Length && char.IsDigit(_expression[_position]))
        {
            _position++;
        }

        // Parse decimal part
        if (_position < _expression.Length && _expression[_position] == '.')
        {
            _position++; // Skip '.'

            if (_position >= _expression.Length || !char.IsDigit(_expression[_position]))
            {
                throw new InvalidOperationException("Invalid number format");
            }

            while (_position < _expression.Length && char.IsDigit(_expression[_position]))
            {
                _position++;
            }
        }

        var numStr = _expression[startPos.._position];

        if (!decimal.TryParse(numStr, out var number))
        {
            throw new InvalidOperationException($"Invalid number: {numStr}");
        }

        return new NumberExpression(number);
    }
}