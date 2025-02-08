using PayVerse.Application.Abstractions.Messaging;

namespace PayVerse.Application.Users.Commands.ChangePassword;

public sealed record ChangePasswordCommand(
    string Email,
    string OldPassword,
    string NewPassword) : ICommand;
