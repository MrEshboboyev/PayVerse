using PayVerse.Application.Abstractions.Messaging;

namespace PayVerse.Application.Invoices.Commands.CreateRecurringInvoice;

public sealed record CreateRecurringInvoiceCommand(
    Guid UserId,
    int FrequencyInMonths,
    List<(string Description, decimal Amount)> Items) : ICommand;