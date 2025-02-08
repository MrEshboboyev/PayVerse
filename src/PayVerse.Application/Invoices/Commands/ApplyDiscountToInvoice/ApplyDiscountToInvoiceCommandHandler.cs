using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Domain.Errors;
using PayVerse.Domain.Repositories;
using PayVerse.Domain.Repositories.Invoices;
using PayVerse.Domain.Shared;
using PayVerse.Domain.ValueObjects;

namespace PayVerse.Application.Invoices.Commands.ApplyDiscountToInvoice;

internal sealed class ApplyDiscountToInvoiceCommandHandler(
    IInvoiceRepository invoiceRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<ApplyDiscountToInvoiceCommand>
{
    public async Task<Result> Handle(
        ApplyDiscountToInvoiceCommand request,
        CancellationToken cancellationToken)
    {
        var (invoiceId, discountAmount) = request;

        #region Get this invoice
        
        var invoice = await invoiceRepository.GetByIdAsync(invoiceId, cancellationToken);
        if (invoice is null)
        {
            return Result.Failure(DomainErrors.Invoice.NotFound(invoiceId));
        }
        
        #endregion
        
        #region Prepare value objects

        var discountResult = Amount.Create(discountAmount);
        if (discountResult.IsFailure)
        {
            return Result.Failure(discountResult.Error);
        }
        
        #endregion
        
        #region Apply discount for this invoice

        var applyDiscountResult = invoice.ApplyDiscount(discountResult.Value);
        if (applyDiscountResult.IsFailure)
        {
            return Result.Failure(applyDiscountResult.Error);
        }
        
        #endregion

        await invoiceRepository.UpdateAsync(invoice, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}