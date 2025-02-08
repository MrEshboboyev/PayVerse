using PayVerse.Application.Abstractions.Messaging;

namespace PayVerse.Application.Users.Commands.AssignRoleToUser;

public sealed record AssignRoleToUserCommand(
    Guid UserId,
    int RoleId) : ICommand;
