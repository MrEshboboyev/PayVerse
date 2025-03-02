using PayVerse.Domain.Decorators.Security;

namespace PayVerse.Application.Decorators.Security;

/// <summary>
/// Base decorator for secure operations
/// </summary>
public abstract class SecureOperationDecorator<TRequest, TResult> : ISecureOperation<TRequest, TResult>
{
    protected readonly ISecureOperation<TRequest, TResult> _decorated;

    protected SecureOperationDecorator(ISecureOperation<TRequest, TResult> decoratedOperation)
    {
        _decorated = decoratedOperation;
    }

    public virtual async Task<TResult> ExecuteAsync(TRequest request, CancellationToken cancellationToken = default)
    {
        return await _decorated.ExecuteAsync(request, cancellationToken);
    }
}
