using PayVerse.Application.Abstractions.Messaging;

namespace PayVerse.Application.Invoices.Commands.CreateRecurringInvoice;

public sealed record CreateRecurringInvoiceCommand(
    string InvoiceNumber,
    DateTime InvoiceDate,
    decimal TotalAmount,
    Guid UserId,
    int FrequencyInMonths) : ICommand;