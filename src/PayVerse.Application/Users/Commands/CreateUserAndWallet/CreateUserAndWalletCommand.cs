using PayVerse.Application.Abstractions.Messaging;

namespace PayVerse.Application.Users.Commands.CreateUserAndWallet;

public sealed record CreateUserAndWalletCommand(
    string Email,
    string Password,
    string FirstName,
    string LastName,
    int RoleId,
    string CurrencyCode) : ICommand;
