namespace PayVerse.Presentation.Contracts.Invoices;

public sealed record CreateRecurringInvoiceRequest(
    int FrequencyInMonths,
    List<(string Description, decimal Amount)> Items);