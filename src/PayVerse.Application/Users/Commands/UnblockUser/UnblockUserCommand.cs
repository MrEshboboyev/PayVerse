using PayVerse.Application.Abstractions.Messaging;

namespace PayVerse.Application.Users.Commands.UnblockUser;

public sealed record UnblockUserCommand(
    Guid UserId) : ICommand;
