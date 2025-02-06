namespace PayVerse.Application.Users.Queries.Common.Responses;

public sealed record UserWithRolesResponse(
    UserResponse User,
    IReadOnlyList<RoleResponse> Roles);