namespace PayVerse.Domain.Decorators.Security;

/// <summary>
/// Defines a generic contract for secure operations
/// </summary>
public interface ISecureOperation<TRequest, TResult>
{
    Task<TResult> ExecuteAsync(TRequest request, CancellationToken cancellationToken = default);
}
