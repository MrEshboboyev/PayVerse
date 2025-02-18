namespace PayVerse.Presentation.Contracts.Users;

public sealed record CreateUserAndWalletRequest(
    string Email,
    string Password,
    string FirstName,
    string LastName,
    string RoleName,
    string Currency);