namespace PayVerse.Presentation.Contracts.Invoices;

public sealed record CreateRecurringInvoiceRequest(
    string InvoiceNumber,
    DateTime InvoiceDate,
    decimal TotalAmount,
    int FrequencyInMonths);