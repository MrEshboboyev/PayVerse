namespace PayVerse.Presentation.Contracts.Users;

public sealed record ChangePasswordRequest(
    string Email,
    string OldPassword,
    string NewPassword);