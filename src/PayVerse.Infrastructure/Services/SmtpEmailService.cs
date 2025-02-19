using MimeKit;
using PayVerse.Application.Interfaces;
using PayVerse.Domain.Errors;
using PayVerse.Domain.Shared;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace PayVerse.Infrastructure.Services;

public class SmtpEmailService(
    string smtpServer,
    int smtpPort,
    string smtpUsername, 
    string smtpPassword) : IEmailService
{
    private readonly string _smtpServer = smtpServer;
    private readonly int _smtpPort = smtpPort;
    private readonly string _smtpUsername = smtpUsername;
    private readonly string _smtpPassword = smtpPassword;

    public async Task<Result> SendEmail(string to, string subject, string body)
    {
        var message = new MimeMessage();
        message.From.Add(MailboxAddress.Parse(_smtpUsername));
        message.To.Add(MailboxAddress.Parse(to));
        message.Subject = subject;

        message.Body = new TextPart("plain")
        {
            Text = body
        };

        using var client = new SmtpClient();
        try
        {
            await client.ConnectAsync(
                _smtpServer, 
                _smtpPort, 
                MailKit.Security.SecureSocketOptions.Auto);
            await client.AuthenticateAsync(_smtpUsername, _smtpPassword);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(
                DomainErrors.Email.SendingFailed(ex.Message));
        }
    }

    public async Task<Result> SendEmailWithAttachment(
        string to,
        string subject, 
        string body, 
        string filePath)
    {
        var message = new MimeMessage();
        message.From.Add(MailboxAddress.Parse(_smtpUsername));
        message.To.Add(MailboxAddress.Parse(to));
        message.Subject = subject;

        var builder = new BodyBuilder
        {
            TextBody = body
        };
        builder.Attachments.Add(filePath);

        message.Body = builder.ToMessageBody();

        using var client = new SmtpClient();
        try
        {
            await client.ConnectAsync(
                _smtpServer, 
                _smtpPort, 
                MailKit.Security.SecureSocketOptions.Auto);
            await client.AuthenticateAsync(_smtpUsername, _smtpPassword);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(
                DomainErrors.Email.SendingFailedWithAttachment(ex.Message));
        }
    }
}