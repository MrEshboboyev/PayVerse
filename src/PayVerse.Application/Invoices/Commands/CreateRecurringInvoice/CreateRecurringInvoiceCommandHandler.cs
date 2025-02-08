using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Domain.Entities.Invoices;
using PayVerse.Domain.Repositories;
using PayVerse.Domain.Repositories.Invoices;
using PayVerse.Domain.Shared;
using PayVerse.Domain.ValueObjects;
using PayVerse.Domain.ValueObjects.Invoices;

namespace PayVerse.Application.Invoices.Commands.CreateRecurringInvoice;

internal sealed class CreateRecurringInvoiceCommandHandler(
    IInvoiceRepository invoiceRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<CreateRecurringInvoiceCommand>
{
    public async Task<Result> Handle(
        CreateRecurringInvoiceCommand request,
        CancellationToken cancellationToken)
    {
        var (invoiceNumber, invoiceDate, totalAmount, userId, frequencyInMonths) = request;

        #region Prepare value objects
        
        var invoiceNumberResult = InvoiceNumber.Create(invoiceNumber);
        if (invoiceNumberResult.IsFailure)
        {
            return Result.Failure(invoiceNumberResult.Error);
        }

        var invoiceDateResult = InvoiceDate.Create(invoiceDate);
        if (invoiceDateResult.IsFailure)
        {
            return Result.Failure(invoiceDateResult.Error);
        }

        var totalAmountResult = Amount.Create(totalAmount);
        if (totalAmountResult.IsFailure)
        {
            return Result.Failure(totalAmountResult.Error);
        }
        
        #endregion
        
        #region Create invoice with recurring 

        var invoice = Invoice.Create(
            Guid.NewGuid(),
            invoiceNumberResult.Value,
            invoiceDateResult.Value,
            totalAmountResult.Value,
            userId);

        var setRecurringResult = invoice.SetRecurring(frequencyInMonths);
        if (setRecurringResult.IsFailure)
        {
            return Result.Failure(setRecurringResult.Error);
        }
        
        #endregion

        await invoiceRepository.AddAsync(invoice, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
