using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Application.Users.Queries.Common.Responses;

namespace PayVerse.Application.Users.Queries.SearchUsers;

public sealed record SearchUsersQuery(
    string? Email = null,
    string? Name = null,
    int? RoleId = null) : IQuery<UserListResponse>;