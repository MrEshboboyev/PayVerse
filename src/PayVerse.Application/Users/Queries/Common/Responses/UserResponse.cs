namespace PayVerse.Application.Users.Queries.Common.Responses;

public sealed record UserResponse(
    Guid Id,
    string Email,
    string FirstName,
    string LastName);
