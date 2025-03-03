using MediatR;
using PayVerse.Application.Facades.Models.Payments;
using PayVerse.Application.Interfaces;
using PayVerse.Application.Invoices.Commands.CancelInvoice;
using PayVerse.Application.Invoices.Commands.CreateInvoice;
using PayVerse.Application.Invoices.Commands.FinalizeInvoice;
using PayVerse.Application.Invoices.Queries.Common.Responses;
using PayVerse.Application.Invoices.Queries.GetInvoiceById;
using PayVerse.Application.Invoices.Queries.GetInvoicesByUserId;
using PayVerse.Application.Payments.Commands.InitiatePayment;
using PayVerse.Application.Payments.Commands.ProcessPayment;
using PayVerse.Application.Payments.Commands.RefundPayment;
using PayVerse.Application.Payments.Queries.Common.Responses;
using PayVerse.Application.Payments.Queries.GetPaymentById;
using PayVerse.Application.Payments.Queries.GetPaymentsByUserId;
using PayVerse.Application.VirtualAccounts.Commands.CreateVirtualAccount;
using PayVerse.Application.VirtualAccounts.Commands.DepositToVirtualAccount;
using PayVerse.Application.VirtualAccounts.Commands.TransferBetweenAccounts;
using PayVerse.Application.VirtualAccounts.Commands.WithdrawFromVirtualAccount;
using PayVerse.Application.VirtualAccounts.Queries.Common.Responses;
using PayVerse.Application.VirtualAccounts.Queries.GetTransactions;
using PayVerse.Application.VirtualAccounts.Queries.GetVirtualAccountById;
using PayVerse.Application.VirtualAccounts.Queries.GetVirtualAccountsByUserId;
using PayVerse.Application.Wallets.Commands.AddFundsToWallet;
using PayVerse.Application.Wallets.Commands.CreateWallet;
using PayVerse.Application.Wallets.Commands.DeductFundsFromWallet;
using PayVerse.Application.Wallets.Queries.Common.Responses;
using PayVerse.Application.Wallets.Queries.GetWalletById;
using PayVerse.Application.Wallets.Queries.GetWalletsByUserId;
using PayVerse.Application.Wallets.Queries.GetWalletTransactions;
using PayVerse.Domain.Enums.Invoices;
using PayVerse.Domain.Enums.Payments;
using PayVerse.Domain.Repositories;
using PayVerse.Domain.Repositories.Invoices;
using PayVerse.Domain.Repositories.Payments;

namespace PayVerse.Application.Facades;


/// <summary>
/// Facade for PayVerse financial operations that simplifies client interactions
/// with the complex subsystems of the application.
/// </summary>
public class PayVerseFacade(
    ISender sender,
    IInvoiceRepository invoiceRepository,
    IPaymentRepository paymentRepository,
    IUnitOfWork unitOfWork) : IPayVerseFacade
{

    #region Invoice Operations

    /// <summary>
    /// Creates a new invoice with the specified details.
    /// </summary>
    public async Task<Guid> CreateInvoiceAsync(CreateInvoiceCommand command)
    {
        var result = await sender.Send(command);
        return result.Value;
    }

    /// <summary>
    /// Gets an invoice by its ID.
    /// </summary>
    public async Task<InvoiceResponse> GetInvoiceByIdAsync(Guid invoiceId)
    {
        var query = new GetInvoiceByIdQuery(invoiceId);
        var result = await sender.Send(query);
        return result.Value;
    }

    /// <summary>
    /// Gets all invoices for a specific user.
    /// </summary>
    public async Task<InvoiceListResponse> GetUserInvoicesAsync(Guid userId)
    {
        var query = new GetInvoicesByUserIdQuery(userId);
        var result = await sender.Send(query);
        return result.Value;
    }

    /// <summary>
    /// Finalizes a draft invoice, changing its status to Issued.
    /// </summary>
    public async Task FinalizeInvoiceAsync(Guid invoiceId)
    {
        var command = new FinalizeInvoiceCommand(invoiceId);
        await sender.Send(command);
    }

    /// <summary>
    /// Cancels an invoice if it's in an appropriate state.
    /// </summary>
    public async Task CancelInvoiceAsync(Guid invoiceId, string reason)
    {
        var command = new CancelInvoiceCommand(invoiceId, reason);
        await sender.Send(command);
    }

    #endregion

    #region Payment Operations

    /// <summary>
    /// Initiates a payment for an invoice.
    /// </summary>
    public async Task<Guid> InitiatePaymentAsync(InitiatePaymentCommand command)
    {
        var result = await sender.Send(command);
        return result.Value;
    }

    /// <summary>
    /// Processes a payment, updating its status to Processed.
    /// </summary>
    public async Task ProcessPaymentAsync(
        Guid paymentId,
        string transactionId,
        string providerName)
    {
        var command = new ProcessPaymentCommand(
            paymentId,
            transactionId,
            new Dictionary<string, string> 
            {
                { "providerName", providerName } 
            });
        await sender.Send(command);
    }

    /// <summary>
    /// Refunds a processed payment.
    /// </summary>
    public async Task RefundPaymentAsync(
        Guid paymentId, 
        string refundTransactionId, 
        string reason)
    {
        var command = new RefundPaymentCommand(paymentId, refundTransactionId, reason);
        await sender.Send(command);
    }

    /// <summary>
    /// Gets a payment by its ID.
    /// </summary>
    public async Task<PaymentResponse> GetPaymentByIdAsync(Guid paymentId)
    {
        var query = new GetPaymentByIdQuery(paymentId);
        var result = await sender.Send(query);
        return result.Value;
    }

    /// <summary>
    /// Gets all payments for a specific user.
    /// </summary>
    public async Task<PaymentListResponse> GetUserPaymentsAsync(Guid userId)
    {
        var query = new GetPaymentsByUserIdQuery(userId);
        var result = await sender.Send(query);
        return result.Value;
    }

    #endregion

    #region VirtualAccount Operations

    /// <summary>
    /// Creates a new virtual account for a user.
    /// </summary>
    public async Task CreateVirtualAccountAsync(CreateVirtualAccountCommand command)
    {
        await sender.Send(command);
    }

    /// <summary>
    /// Deposits funds into a virtual account.
    /// </summary>
    public async Task DepositToVirtualAccountAsync(
        Guid accountId, 
        decimal amount,
        string description)
    {
        var command = new DepositToVirtualAccountCommand(accountId, amount, description);
        await sender.Send(command);
    }

    /// <summary>
    /// Withdraws funds from a virtual account.
    /// </summary>
    public async Task WithdrawFromVirtualAccountAsync(
        Guid accountId, 
        decimal amount, 
        string description)
    {
        var command = new WithdrawFromVirtualAccountCommand(accountId, amount, description);
        await sender.Send(command);
    }

    /// <summary>
    /// Transfers funds between two virtual accounts.
    /// </summary>
    public async Task TransferBetweenAccountsAsync(
        Guid fromAccountId, 
        Guid toAccountId, 
        decimal amount, 
        string description)
    {
        var command = new TransferBetweenAccountsCommand(fromAccountId, toAccountId, amount, description);
        await sender.Send(command);
    }

    /// <summary>
    /// Gets a virtual account by its ID.
    /// </summary>
    public async Task<VirtualAccountResponse> GetVirtualAccountByIdAsync(Guid accountId)
    {
        var query = new GetVirtualAccountByIdQuery(accountId);
        var result = await sender.Send(query);
        return result.Value;
    }

    /// <summary>
    /// Gets all virtual accounts for a specific user.
    /// </summary>
    public async Task<VirtualAccountListResponse> GetUserVirtualAccountsAsync(Guid userId)
    {
        var query = new GetVirtualAccountsByUserIdQuery(userId);
        var result = await sender.Send(query);
        return result.Value;
    }

    /// <summary>
    /// Gets transactions for a specific virtual account.
    /// </summary>
    public async Task<TransactionListResponse> GetVirtualAccountTransactionsAsync(Guid accountId)
    {
        var query = new GetTransactionsQuery(accountId);
        var result = await sender.Send(query);
        return result.Value;
    }

    #endregion

    #region Wallet Operations

    /// <summary>
    /// Creates a new wallet for a user.
    /// </summary>
    public async Task CreateWalletAsync(CreateWalletCommand command)
    {
        await sender.Send(command);
    }

    /// <summary>
    /// Adds funds to a wallet.
    /// </summary>
    public async Task AddFundsToWalletAsync(
        Guid walletId, 
        decimal amount, 
        string description)
    {
        var command = new AddFundsToWalletCommand(walletId, amount, description);
        await sender.Send(command);
    }

    /// <summary>
    /// Deducts funds from a wallet.
    /// </summary>
    public async Task DeductFundsFromWalletAsync(
        Guid walletId, 
        decimal amount, 
        string description)
    {
        var command = new DeductFundsFromWalletCommand(walletId, amount, description);
        await sender.Send(command);
    }

    /// <summary>
    /// Gets a wallet by its ID.
    /// </summary>
    public async Task<WalletResponse> GetWalletByIdAsync(Guid walletId)
    {
        var query = new GetWalletByIdQuery(walletId);
        var result = await sender.Send(query);
        return result.Value;
    }

    /// <summary>
    /// Gets all wallets for a specific user.
    /// </summary>
    public async Task<WalletListResponse> GetUserWalletsAsync(Guid userId)
    {
        var query = new GetWalletsByUserIdQuery(userId);
        var result = await sender.Send(query);
        return result.Value;
    }

    /// <summary>
    /// Gets transactions for a specific wallet.
    /// </summary>
    public async Task<WalletTransactionListResponse> GetWalletTransactionsAsync(Guid walletId)
    {
        var query = new GetWalletTransactionsQuery(walletId);
        var result = await sender.Send(query);
        return result.Value;
    }

    #endregion

    #region Composite Operations

    /// <summary>
    /// Creates an invoice and immediately schedules a payment for it.
    /// This is a complex operation that spans multiple aggregates but is simplified through the facade.
    /// </summary>
    public async Task<InvoicePaymentResultDto> CreateInvoiceWithScheduledPaymentAsync(
        CreateInvoiceCommand createInvoiceCommand,
        DateTime scheduledPaymentDate,
        PaymentMethod paymentMethod)
    {
        try
        {
            // Begin transaction
            await unitOfWork.BeginTransactionAsync();

            // Create invoice
            var invoiceId = await CreateInvoiceAsync(createInvoiceCommand);

            var invoice = await invoiceRepository.GetByIdAsync(
                invoiceId, 
                CancellationToken.None);

            // Finalize invoice
            await FinalizeInvoiceAsync(invoice.Id);

            // Schedule payment
            var paymentCommand = new InitiatePaymentCommand(
                invoice.Items.Sum(i => i.Amount.Value),
                invoice.UserId,
                invoice.Id,
                paymentMethod,
                scheduledPaymentDate
            );

            var paymentId = await InitiatePaymentAsync(paymentCommand);

            // Commit transaction
            await unitOfWork.CommitTransactionAsync();

            return new InvoicePaymentResultDto(
                invoice.Id,
                invoice.InvoiceNumber.ToString()!,
                paymentId,
                scheduledPaymentDate,
                "Scheduled"
            );
        }
        catch (Exception)
        {
            // Rollback transaction in case of error
            await unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }

    /// <summary>
    /// Pays an invoice using funds from a virtual account.
    /// Another complex operation simplified through the facade.
    /// </summary>
    public async Task<PaymentResultDto> PayInvoiceFromVirtualAccountAsync(
        Guid invoiceId,
        Guid virtualAccountId,
        string description)
    {
        try
        {
            // Begin transaction
            await unitOfWork.BeginTransactionAsync();

            // Get invoice
            var invoice = await invoiceRepository.GetByIdAsync(
                invoiceId,
                CancellationToken.None);
            if (invoice.Status != InvoiceStatus.Issued)
            {
                throw new InvalidOperationException("Invoice must be in Issued status to be paid");
            }

            // Get virtual account
            var account = await GetVirtualAccountByIdAsync(virtualAccountId);

            // Withdraw from virtual account
            await WithdrawFromVirtualAccountAsync(
                virtualAccountId, 
                invoice.Items.Sum(i => i.Amount.Value),
                $"Payment for Invoice #{invoice.InvoiceNumber}");

            // Create payment
            var paymentCommand = new InitiatePaymentCommand(
                invoice.Items.Sum(i => i.Amount.Value),
                invoice.UserId,
                invoice.Id,
                PaymentMethod.VirtualAccount
            );

            var paymentId = await InitiatePaymentAsync(paymentCommand);

            var payment = await paymentRepository.GetByIdAsync(
                paymentId,
                CancellationToken.None);

            // Process payment
            await ProcessPaymentAsync(
                payment.Id, 
                Guid.NewGuid().ToString(),
                "PayVerse");

            // Commit transaction
            await unitOfWork.CommitTransactionAsync();

            return new PaymentResultDto(
                true,
                payment.Id,
                invoice.Id,
                payment.Amount.Value,
                DateTime.UtcNow,
                description,
                "NONE"
            );
        }
        catch (Exception)
        {
            // Rollback transaction in case of error
            await unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }

    #endregion
}