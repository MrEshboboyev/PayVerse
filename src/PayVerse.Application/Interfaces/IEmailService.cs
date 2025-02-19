using PayVerse.Domain.Shared;

namespace PayVerse.Application.Interfaces;

public interface IEmailService
{
    Task<Result> SendEmail(string to, string subject, string body);
    Task<Result> SendEmailWithAttachment(string to, string subject, string body, string filePath);
}