namespace PayVerse.Infrastructure.Services.Security;


// ✅ Benefits:
// Prevents multiple logging instances that could create duplicate audit records.
// Ensures consistent logging structure for all actions.
public sealed class AuditLogService
{
    private static readonly Lazy<AuditLogService> _instance = 
        new(() => new AuditLogService());

    private AuditLogService() { }

    public static AuditLogService Instance => _instance.Value;

    public void LogAction(string action, Guid userId) =>
        Console.WriteLine($"[AUDIT] User {userId} - {action} at {DateTime.UtcNow}");
}
