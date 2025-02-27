using PayVerse.Domain.Entities.Invoices;
using PayVerse.Domain.ValueObjects;
using PayVerse.Domain.ValueObjects.Invoices;

namespace PayVerse.Domain.Builders.Invoices;

/// <summary>
/// Builder for creating Invoice entities
/// </summary>
/// <remarks>
/// Constructor with required parameters
/// </remarks>
public class InvoiceBuilder(Guid userId) : IBuilder<Invoice>
{
    #region Private Properties

    // Optional parameters with default values
    private InvoiceNumber _invoiceNumber = InvoiceNumber.Generate();
    private InvoiceDate _invoiceDate = InvoiceDate.Create(DateTime.UtcNow).Value;
    private Amount _totalAmount = Amount.Create(0).Value;
    private readonly List<InvoiceItemDto> _items = [];
    private int? _recurringFrequencyInMonths;

    #endregion

    #region Building Blocks

    /// <summary>
    /// Sets the invoice number
    /// </summary>
    public InvoiceBuilder WithInvoiceNumber()
    {
        _invoiceNumber = InvoiceNumber.Generate();
        return this;
    }

    /// <summary>
    /// Sets the invoice date
    /// </summary>
    public InvoiceBuilder WithInvoiceDate(DateTime date)
    {
        _invoiceDate = InvoiceDate.Create(date).Value;
        return this;
    }

    /// <summary>
    /// Sets the invoice as recurring with a specified frequency
    /// </summary>
    public InvoiceBuilder AsRecurring(int frequencyInMonths)
    {
        if (frequencyInMonths <= 0)
        {
            throw new ArgumentException("Recurring frequency must be a positive value");
        }

        _recurringFrequencyInMonths = frequencyInMonths;
        return this;
    }

    /// <summary>
    /// Sets the currency for the invoice
    /// </summary>
    public InvoiceBuilder WithCurrency()
    {
        _totalAmount = Amount.Create(_totalAmount.Value).Value; // currency handle - coming soon
        return this;
    }

    #endregion

    #region Items

    /// <summary>
    /// Adds an item to the invoice
    /// </summary>
    public InvoiceBuilder AddItem(string description, decimal amount)
    {
        _items.Add(new InvoiceItemDto(description, amount));
        return this;
    }

    /// <summary>
    /// Adds multiple items to the invoice
    /// </summary>
    public InvoiceBuilder AddItems(IEnumerable<InvoiceItemDto> items)
    {
        _items.AddRange(items);
        return this;
    }

    #endregion

    #region Build Construct

    /// <summary>
    /// Builds the Invoice instance
    /// </summary>
    public Invoice Build()
    {
        // Calculate total from items
        decimal totalAmount = _items.Sum(i => i.Amount.Value);

        // Create invoice with factory method
        var invoice = Invoice.Create(
            Guid.NewGuid(),
            _invoiceNumber,
            _invoiceDate,
            Amount.Create(totalAmount).Value,
            userId);

        // Add items
        foreach (var item in _items)
        {
            invoice.AddItem(item.Description, item.Amount);
        }

        // Set recurring if specified
        if (_recurringFrequencyInMonths.HasValue)
        {
            invoice.SetRecurringFrequency(_recurringFrequencyInMonths.Value);
        }

        return invoice;
    }

    #endregion
}