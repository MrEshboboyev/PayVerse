using PayVerse.Application.Common.Interfaces;
using PayVerse.Application.Decorators.Security;
using PayVerse.Domain.Decorators.Security;

namespace PayVerse.Application.Security.Decorators;

/// <summary>
/// Adds authorization checks to operations
/// </summary>
public class AuthorizationDecorator<TRequest, TResult>(
    ISecureOperation<TRequest, TResult> decoratedOperation) : SecureOperationDecorator<TRequest, TResult>(decoratedOperation)
{
    public override async Task<TResult> ExecuteAsync(TRequest request, CancellationToken cancellationToken = default)
    {
        //// Check if user has the required permissions
        //var hasPermissions = await authorizationService.HasPermissionAsync(
        //    currentUserService.UserId,
        //    requiredPermissions.First(), // fix this coming soon
        //    cancellationToken);

        //if (!hasPermissions)
        //{
        //    throw new ForbiddenAccessException("User does not have the required permissions to perform this operation");
        //}

        return await base.ExecuteAsync(request, cancellationToken);
    }
}

public class ForbiddenAccessException(string message) : Exception(message)
{
}