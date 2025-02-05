using PayVerse.Application.Abstractions.Messaging;

namespace PayVerse.Application.Users.Commands.Login;

public sealed record LoginCommand(
    string Email, 
    string Password) : ICommand<string>;
