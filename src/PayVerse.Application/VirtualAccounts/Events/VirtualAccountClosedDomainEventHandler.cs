using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Domain.Events.VirtualAccounts;
using PayVerse.Domain.Repositories.VirtualAccounts;

namespace PayVerse.Application.VirtualAccounts.Events;

internal sealed class VirtualAccountClosedDomainEventHandler(
    IVirtualAccountRepository virtualAccountRepository) : IDomainEventHandler<VirtualAccountClosedDomainEvent>
{
    public async Task Handle(
        VirtualAccountClosedDomainEvent notification,
        CancellationToken cancellationToken)
    {
        var account = await virtualAccountRepository.GetByIdAsync(
            notification.VirtualAccountId,
            cancellationToken);
        if (account is null)
        {
            return;
        }

        Console.WriteLine($"Virtual account [ID : {account.Id}] has been closed.");
    }
}
