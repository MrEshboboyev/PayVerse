using PayVerse.Application.Abstractions.Messaging;

namespace PayVerse.Application.Users.Commands.ResetPassword;

public sealed record ResetPasswordCommand(
    Guid UserId,
    string NewPassword) : ICommand;
