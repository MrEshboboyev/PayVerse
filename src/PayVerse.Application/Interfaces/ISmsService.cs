using PayVerse.Domain.Shared;

namespace PayVerse.Application.Interfaces;

public interface ISmsService
{
    Task<Result> SendSms(string phoneNumber, string message);
}