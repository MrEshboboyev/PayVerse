using PayVerse.Application.Abstractions.Messaging;

namespace PayVerse.Application.Users.Commands.BlockUser;

public sealed record BlockUserCommand(
    Guid UserId) : ICommand;
