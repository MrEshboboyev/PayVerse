using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Domain.Builders.Invoices;
using PayVerse.Domain.Repositories;
using PayVerse.Domain.Repositories.Invoices;
using PayVerse.Domain.Shared;
using PayVerse.Domain.ValueObjects;

namespace PayVerse.Application.Invoices.Commands.CreateInvoice;

internal sealed class CreateInvoiceCommandHandler(
    IInvoiceRepository invoiceRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<CreateInvoiceCommand>
{
    public async Task<Result> Handle(
        CreateInvoiceCommand request,
        CancellationToken cancellationToken)
    {
        var (userId, items) = request;

        #region Create Invoice using Builder

        var invoiceBuilder = new InvoiceBuilder(userId);

        foreach (var item in items)
        {
            var (description, amount) = item;

            var amountResult = Amount.Create(amount);
            if (amountResult.IsFailure)
            {
                return Result.Failure(amountResult.Error);
            }
            invoiceBuilder.AddItem(description, amountResult.Value);
        }

        var invoice = invoiceBuilder.Build();

        #endregion

        #region Add Invoice to Repository

        await invoiceRepository.AddAsync(invoice, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        #endregion

        return Result.Success();
    }
}
