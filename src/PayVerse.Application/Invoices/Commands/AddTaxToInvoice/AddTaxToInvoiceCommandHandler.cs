using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Domain.Errors;
using PayVerse.Domain.Repositories;
using PayVerse.Domain.Repositories.Invoices;
using PayVerse.Domain.Shared;
using PayVerse.Domain.ValueObjects;

namespace PayVerse.Application.Invoices.Commands.AddTaxToInvoice;

internal sealed class AddTaxToInvoiceCommandHandler(
    IInvoiceRepository invoiceRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<AddTaxToInvoiceCommand>
{
    public async Task<Result> Handle(
        AddTaxToInvoiceCommand request,
        CancellationToken cancellationToken)
    {
        var (invoiceId, taxAmount) = request;
        
        #region Get this Invoice

        var invoice = await invoiceRepository.GetByIdAsync(invoiceId, cancellationToken);
        if (invoice is null)
        {
            return Result.Failure(DomainErrors.Invoice.NotFound(invoiceId));
        }
        
        #endregion
        
        #region Prepare value objects

        var taxResult = Amount.Create(taxAmount);
        if (taxResult.IsFailure)
        {
            return Result.Failure(taxResult.Error);
        }
        
        #endregion
        
        #region Add Tax

        var addTaxResult = invoice.AddTax(taxResult.Value);
        if (addTaxResult.IsFailure)
        {
            return Result.Failure(addTaxResult.Error);
        }
        
        #endregion

        await invoiceRepository.UpdateAsync(invoice, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}