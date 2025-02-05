﻿namespace PayVerse.Application.Users.Queries.GetUserById;

public sealed record UserResponse(
    Guid Id,
    string Email,
    string FirstName,
    string LastName,
    IEnumerable<string> RoleName);
