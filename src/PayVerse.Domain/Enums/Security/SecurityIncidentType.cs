namespace PayVerse.Domain.Enums.Security;

public enum SecurityIncidentType
{
    FailedLogin = 100,
    SuspiciousTransaction = 200,
    UnauthorizedAccess = 300,
    DataBreachAttempt = 400,
    PotentialFraud = 500,
    BlockedIpAttempt = 600,
    BlockedUserLoginAttempt = 700
}