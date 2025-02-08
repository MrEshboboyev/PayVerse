using PayVerse.Application.Abstractions.Messaging;

namespace PayVerse.Application.Users.Commands.UpdateUser;

public sealed record UpdateUserCommand(
    Guid UserId,
    string FirstName,
    string LastName) : ICommand;
