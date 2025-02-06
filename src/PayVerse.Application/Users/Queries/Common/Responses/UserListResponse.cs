namespace PayVerse.Application.Users.Queries.Common.Responses;

public sealed record UserListResponse(IReadOnlyList<UserResponse> Users);