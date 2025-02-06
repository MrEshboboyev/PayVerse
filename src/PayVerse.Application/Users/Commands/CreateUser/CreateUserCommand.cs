using PayVerse.Application.Abstractions.Messaging;

namespace PayVerse.Application.Users.Commands.CreateUser;

public sealed record CreateUserCommand(
    string Email,
    string Password,
    string FirstName,
    string LastName,
    int RoleId) : ICommand<Guid>;
