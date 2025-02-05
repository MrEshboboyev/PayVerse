using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Domain.Events.VirtualAccounts;
using PayVerse.Domain.Repositories.VirtualAccounts;

namespace PayVerse.Application.VirtualAccounts.Events;

internal sealed class TransactionAddedDomainEventHandler(
    IVirtualAccountRepository virtualAccountRepository) : IDomainEventHandler<TransactionAddedDomainEvent>
{
    public async Task Handle(
        TransactionAddedDomainEvent notification,
        CancellationToken cancellationToken)
    {
        #region Get this VirtualAccount
        
        var virtualAccount = await virtualAccountRepository.GetByIdAsync(
            notification.Id,
            cancellationToken);

        if (virtualAccount is null)
        {
            return;
        }
        
        #endregion
        
        #region Get this item from Invoice
        
        var transaction = virtualAccount.GetTransactionById(notification.TransactionId);
        if (transaction is null)
        {
            return;
        }
        
        #endregion

        Console.WriteLine();
        
        Console.WriteLine($"Transaction added with amount : {transaction.Amount.Value}" +
                          $" to this virtual account [ID : {virtualAccount.Id}]");
    }
}