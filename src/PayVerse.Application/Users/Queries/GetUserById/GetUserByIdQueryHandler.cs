using PayVerse.Application.Abstractions.Messaging;
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
        // Fetch the user from the repository
        var user = await userRepository.GetByIdAsync(request.UserId, cancellationToken);   

        // If user is not found, return a failure result
        if (user is null)
        {
            return Result.Failure<UserResponse>(
                DomainErrors.User.NotFound(request.UserId));
        }

        // Create and return the UserResponse object
        var response = new UserResponse(
            user.Id,
            user.Email.Value,
            user.FirstName.Value,
            user.LastName.Value,
            user.Roles.Select(role => role.Name.ToString()));

        return response;
    }
}
