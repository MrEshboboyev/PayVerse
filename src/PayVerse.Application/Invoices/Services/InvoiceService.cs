using PayVerse.Domain.Builders.Invoices;
using PayVerse.Domain.Entities.Invoices;
using PayVerse.Domain.Prototypes;
using PayVerse.Domain.Repositories.Invoices;
using PayVerse.Domain.ValueObjects;

namespace PayVerse.Application.Invoices.Services;

// Example usage in application service
public class InvoiceService(PrototypeRegistry prototypeRegistry,
                            IInvoiceRepository invoiceRepository)
{
    #region Prototypes

    public async Task<Guid> CreateRecurringInvoiceFromTemplate(string templateKey,
                                                               int frequencyInMonths)
    {
        // Get prototype from registry
        var prototype = prototypeRegistry.GetPrototype(templateKey) as Invoice
            ?? throw new InvalidOperationException($"Template with key '{templateKey}' is not an Invoice");

        // Create new invoice from prototype with recurring frequency
        var newInvoice = Invoice.CreateRecurringFromPrototype(prototype, frequencyInMonths);

        // Save new invoice
        await invoiceRepository.AddAsync(newInvoice);

        return newInvoice.Id;
    }

    public async Task RegisterInvoiceTemplate(Guid invoiceId, string templateKey)
    {
        var invoice = await invoiceRepository.GetByIdAsync(invoiceId)
            ?? throw new ArgumentException(nameof(Invoice));

        // Register invoice as prototype
        prototypeRegistry.RegisterPrototype(templateKey, invoice);
    }

    #endregion

    #region Builders

    /// <summary>
    /// Creates a standard invoice with default settings
    /// </summary>
    public async Task<Guid> CreateStandardInvoice(Guid userId,
                                                  IEnumerable<InvoiceItemDto> items,
                                                  Currency currency)
    {
        var invoice = Invoice.CreateBuilder(userId)
            .WithCurrency() // add currency - coming soon
            .AddItems(items)
            .Build();

        await invoiceRepository.AddAsync(invoice);

        return invoice.Id;
    }

    /// <summary>
    /// Creates a recurring invoice
    /// </summary>
    public async Task<Guid> CreateRecurringInvoice(Guid userId,
                                                   IEnumerable<InvoiceItemDto> items,
                                                   Currency currency,
                                                   int frequencyInMonths)
    {
        var invoice = Invoice.CreateBuilder(userId)
            .WithCurrency() // add currency - coming soon
            .AsRecurring(frequencyInMonths)
            .AddItems(items)
            .Build();

        await invoiceRepository.AddAsync(invoice);

        return invoice.Id;
    }

    #endregion
}