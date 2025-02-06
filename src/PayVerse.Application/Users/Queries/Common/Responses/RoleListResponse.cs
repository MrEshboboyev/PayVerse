namespace PayVerse.Application.Users.Queries.Common.Responses;

public sealed record RoleListResponse(IReadOnlyCollection<RoleResponse> Roles);