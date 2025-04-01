namespace PayVerse.Presentation.Contracts.Admins;

public sealed record SearchUsersRequest(
    string Email = null,
    string Name = null,
    int? RoleId = null);