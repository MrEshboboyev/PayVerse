using PayVerse.Application.Common.Interfaces;

namespace PayVerse.Infrastructure.Services;

public class LoggingService : ILoggingService
{
    public void LogError(string message)
    {
        Console.WriteLine($"[ERROR] {DateTime.UtcNow}: {message}");
    }

    public void LogInfo(string message)
    {
        Console.WriteLine($"[INFO] {DateTime.UtcNow}: {message}");
    }

    public void LogWarning(string message)
    {
        Console.WriteLine($"[WARNING] {DateTime.UtcNow}: {message}");
    }
}