using PayVerse.Application.Common.Interfaces;
using PayVerse.Application.Decorators.Security;
using PayVerse.Domain.Decorators.Security;

namespace PayVerse.Application.Security.Decorators;

/// <summary>
/// Adds authentication verification to operations
/// </summary>
public class AuthenticationDecorator<TRequest, TResult>(
    ISecureOperation<TRequest, TResult> decoratedOperation,
    ICurrentUserService currentUserService,
    IAuthenticationService authenticationService) : SecureOperationDecorator<TRequest, TResult>(decoratedOperation)
{
    public override async Task<TResult> ExecuteAsync(TRequest request, CancellationToken cancellationToken = default)
    {
        if (!currentUserService.IsAuthenticated)
        {
            throw new UnauthorizedAccessException("User must be authenticated to perform this operation");
        }

        //// Verify token validity
        //var isValidToken = await authenticationService.ValidateTokenAsync(
        //    currentUserService.AuthToken,
        //    cancellationToken);

        //if (!isValidToken)
        //{
        //    throw new UnauthorizedAccessException("Invalid or expired authentication token");
        //}

        return await base.ExecuteAsync(request, cancellationToken);
    }
}
