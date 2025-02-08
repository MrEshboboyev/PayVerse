using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Domain.Events.VirtualAccounts;
using PayVerse.Domain.Repositories.VirtualAccounts;

namespace PayVerse.Application.VirtualAccounts.Events;

internal sealed class VirtualAccountFrozenDomainEventHandler(
    IVirtualAccountRepository virtualAccountRepository) : IDomainEventHandler<VirtualAccountFrozenDomainEvent>
{
    public async Task Handle(
        VirtualAccountFrozenDomainEvent notification,
        CancellationToken cancellationToken)
    {
        var account = await virtualAccountRepository.GetByIdAsync(
            notification.AccountId,
            cancellationToken);
        if (account is null)
        {
            return;
        }

        Console.WriteLine($"Virtual account [ID : {account.Id}] has been frozen.");
    }
}
