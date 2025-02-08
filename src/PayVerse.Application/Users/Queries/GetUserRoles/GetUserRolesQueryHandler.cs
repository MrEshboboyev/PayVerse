using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Application.Users.Queries.Common.Factories;
using PayVerse.Application.Users.Queries.Common.Responses;
using PayVerse.Domain.Errors;
using PayVerse.Domain.Repositories.Users;
using PayVerse.Domain.Shared;

namespace PayVerse.Application.Users.Queries.GetUserRoles;

internal sealed class GetUserRolesQueryHandler(IUserRepository userRepository)
    : IQueryHandler<GetUserRolesQuery, RoleListResponse>
{
    public async Task<Result<RoleListResponse>> Handle(
        GetUserRolesQuery request,
        CancellationToken cancellationToken)
    {
        var userId = request.UserId;
        
        var user = await userRepository.GetByIdWithRolesAsync(
            userId,
            cancellationToken);

        if (user is null)
        {
            return Result.Failure<RoleListResponse>(
                DomainErrors.User.NotFound(userId));
        }

        var response = new RoleListResponse(
            user.Roles
                .Select(RoleResponseFactory.Create)
                .ToList()
                .AsReadOnly());

        return Result.Success(response);
    }
}