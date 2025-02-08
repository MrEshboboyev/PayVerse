namespace PayVerse.Presentation.Contracts.Users;

public sealed record ResetPasswordRequest(
    Guid UserId,
    string NewPassword);