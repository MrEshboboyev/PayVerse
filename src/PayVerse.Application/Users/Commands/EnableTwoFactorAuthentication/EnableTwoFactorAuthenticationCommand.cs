using PayVerse.Application.Abstractions.Messaging;

namespace PayVerse.Application.Users.Commands.EnableTwoFactorAuthentication;

public sealed record EnableTwoFactorAuthenticationCommand(
    Guid UserId) : ICommand;
