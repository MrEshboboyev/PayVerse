using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Application.Users.Queries.Common.Responses;

namespace PayVerse.Application.Users.Queries.GetAllUsers;

public sealed record GetAllUsersQuery() : IQuery<UserListResponse>;