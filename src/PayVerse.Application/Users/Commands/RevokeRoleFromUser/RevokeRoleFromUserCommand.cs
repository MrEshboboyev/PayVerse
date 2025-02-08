using PayVerse.Application.Abstractions.Messaging;

namespace PayVerse.Application.Users.Commands.RevokeRoleFromUser;

public sealed record RevokeRoleFromUserCommand(
    Guid UserId,
    int RoleId) : ICommand;
