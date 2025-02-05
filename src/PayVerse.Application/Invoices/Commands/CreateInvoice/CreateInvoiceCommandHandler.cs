using InspireEd.Domain.Repositories;
using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Domain.Entities.Invoices;
using PayVerse.Domain.Repositories.Invoices;
using PayVerse.Domain.Shared;
using PayVerse.Domain.ValueObjects.Invoices;

namespace PayVerse.Application.Invoices.Commands.CreateInvoice;

internal sealed class CreateInvoiceCommandHandler(
    IInvoiceRepository invoiceRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<CreateInvoiceCommand>
{
    public async Task<Result> Handle(
        CreateInvoiceCommand request,
        CancellationToken cancellationToken)
    {
        var (invoiceNumber, invoiceDate, totalAmount, userId) = request;
        
        #region Prepare value objects
        
        var invoiceNumberResult = InvoiceNumber.Create(invoiceNumber);
        if (invoiceNumberResult.IsFailure)
        {
            return Result.Failure(
                invoiceNumberResult.Error);
        }
        
        var invoiceDateResult = InvoiceDate.Create(invoiceDate);
        if (invoiceDateResult.IsFailure)
        {
            return Result.Failure(
                invoiceDateResult.Error);
        }
        
        var totalAmountResult = Amount.Create(totalAmount);
        if (totalAmountResult.IsFailure)
        {
            return Result.Failure(
                totalAmountResult.Error);
        }
        
        #endregion 
        
        #region Create Invoice

        var invoice = Invoice.Create(
            Guid.NewGuid(),
            invoiceNumberResult.Value,
            invoiceDateResult.Value,
            totalAmountResult.Value,
            userId);

        #endregion

        #region Add Invoice to Repository

        await invoiceRepository.AddAsync(invoice, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        #endregion

        return Result.Success();
    }
}