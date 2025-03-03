using PayVerse.Application.Facades.Models.Payments;
using PayVerse.Application.Invoices.Commands.CreateInvoice;
using PayVerse.Application.Invoices.Queries.Common.Responses;
using PayVerse.Application.Payments.Commands.InitiatePayment;
using PayVerse.Application.Payments.Queries.Common.Responses;
using PayVerse.Application.VirtualAccounts.Commands.CreateVirtualAccount;
using PayVerse.Application.VirtualAccounts.Queries.Common.Responses;
using PayVerse.Application.Wallets.Commands.CreateWallet;
using PayVerse.Application.Wallets.Queries.Common.Responses;
using PayVerse.Domain.Enums.Payments;

namespace PayVerse.Application.Interfaces;

/// <summary>
/// Interface for the PayVerse Facade that provides a simplified interface
/// to the complex subsystems of the application.
/// </summary>
public interface IPayVerseFacade
{
    #region Invoice Operations

    Task<Guid> CreateInvoiceAsync(CreateInvoiceCommand command);
    Task<InvoiceResponse> GetInvoiceByIdAsync(Guid invoiceId);
    Task<InvoiceListResponse> GetUserInvoicesAsync(Guid userId);
    Task FinalizeInvoiceAsync(Guid invoiceId);
    Task CancelInvoiceAsync(Guid invoiceId, string reason);
    
    #endregion

    #region Payment Operations
    
    Task<Guid> InitiatePaymentAsync(InitiatePaymentCommand command);
    Task ProcessPaymentAsync(Guid paymentId, string transactionId, string providerName);
    Task RefundPaymentAsync(Guid paymentId, string refundTransactionId, string reason);
    Task<PaymentResponse> GetPaymentByIdAsync(Guid paymentId);
    Task<PaymentListResponse> GetUserPaymentsAsync(Guid userId);
    
    #endregion

    #region VirtualAccount Operations
    
    Task CreateVirtualAccountAsync(CreateVirtualAccountCommand command);
    Task DepositToVirtualAccountAsync(Guid accountId, decimal amount, string description);
    Task WithdrawFromVirtualAccountAsync(Guid accountId, decimal amount, string description);
    Task TransferBetweenAccountsAsync(Guid fromAccountId, Guid toAccountId, decimal amount, string description);
    Task<VirtualAccountResponse> GetVirtualAccountByIdAsync(Guid accountId);
    Task<VirtualAccountListResponse> GetUserVirtualAccountsAsync(Guid userId);
    Task<TransactionListResponse> GetVirtualAccountTransactionsAsync(Guid accountId);
    
    #endregion

    #region Wallet Operations
    
    Task CreateWalletAsync(CreateWalletCommand command);
    Task AddFundsToWalletAsync(Guid walletId, decimal amount, string description);
    Task DeductFundsFromWalletAsync(Guid walletId, decimal amount, string description);
    Task<WalletResponse> GetWalletByIdAsync(Guid walletId);
    Task<WalletListResponse> GetUserWalletsAsync(Guid userId);
    Task<WalletTransactionListResponse> GetWalletTransactionsAsync(Guid walletId);
    
    #endregion

    #region Composite Operations

    Task<InvoicePaymentResultDto> CreateInvoiceWithScheduledPaymentAsync(
        CreateInvoiceCommand createInvoiceCommand,
        DateTime scheduledPaymentDate,
        PaymentMethod paymentMethod);

    Task<PaymentResultDto> PayInvoiceFromVirtualAccountAsync(
        Guid invoiceId,
        Guid virtualAccountId,
        string description);
    
    #endregion
}