using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Application.Users.Queries.Common;
using PayVerse.Application.Users.Queries.Common.Factories;
using PayVerse.Application.Users.Queries.Common.Responses;
using PayVerse.Domain.Errors;
using PayVerse.Domain.Repositories;
using PayVerse.Domain.Repositories.Users;
using PayVerse.Domain.Shared;

namespace PayVerse.Application.Users.Queries.GetUserById;

internal sealed class GetUserByIdQueryHandler(IUserRepository userRepository)
    : IQueryHandler<GetUserByIdQuery, UserResponse>
{
    public async Task<Result<UserResponse>> Handle(
        GetUserByIdQuery request,
        CancellationToken cancellationToken)
    {
        var userId = request.UserId;
        
        // Fetch the user from the repository
        var user = await userRepository.GetByIdAsync(userId, cancellationToken);   

        // If user is not found, return a failure result
        if (user is null)
        {
            return Result.Failure<UserResponse>(
                DomainErrors.User.NotFound(userId));
        }
        
        #region Prepare response
        
        var response = UserResponseFactory.Create(user);
        
        #endregion

        return Result.Success(response);
    }
}
