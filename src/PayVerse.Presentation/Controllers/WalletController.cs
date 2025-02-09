using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayVerse.Application.Wallets.Commands.AddWalletTransaction;
using PayVerse.Application.Wallets.Commands.ConvertWalletCurrency;
using PayVerse.Application.Wallets.Commands.CreateWallet;
using PayVerse.Application.Wallets.Commands.RedeemLoyaltyPoints;
using PayVerse.Application.Wallets.Commands.RemoveWalletTransaction;
using PayVerse.Application.Wallets.Commands.SetSpendingLimit;
using PayVerse.Application.Wallets.Queries.GetLoyaltyPointsBalance;
using PayVerse.Application.Wallets.Queries.GetWalletBalanceByCurrency;
using PayVerse.Application.Wallets.Queries.GetWalletById;
using PayVerse.Application.Wallets.Queries.GetWalletsByUserId;
using PayVerse.Application.Wallets.Queries.GetWalletTransactionById;
using PayVerse.Application.Wallets.Queries.GetWalletTransactions;
using PayVerse.Application.Wallets.Queries.GetWalletWithTransactionsById;
using PayVerse.Presentation.Abstractions;
using PayVerse.Presentation.Contracts.Wallets;

namespace PayVerse.Presentation.Controllers;

[Route("api/wallets")]
[Authorize]
public sealed class WalletController(ISender sender) : ApiController(sender)
{
    #region Get Endpoints

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetWalletById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var query = new GetWalletByIdQuery(id);
        var response = await Sender.Send(query, cancellationToken);
        return response.IsSuccess ? Ok(response.Value) : NotFound(response.Error);
    }

    [HttpGet("user")]
    public async Task<IActionResult> GetWallets(
        CancellationToken cancellationToken)
    {
        var query = new GetWalletsByUserIdQuery(GetUserId());
        var response = await Sender.Send(query, cancellationToken);
        return response.IsSuccess ? Ok(response.Value) : NotFound(response.Error);
    }

    [HttpGet("{walletId:guid}/transactions")]
    public async Task<IActionResult> GetWalletTransactions(
        Guid walletId,
        CancellationToken cancellationToken)
    {
        var query = new GetWalletTransactionsQuery(walletId);
        var response = await Sender.Send(query, cancellationToken);
        return response.IsSuccess ? Ok(response.Value) : NotFound(response.Error);
    }

    [HttpGet("{walletId:guid}/transactions/{transactionId:guid}")]
    public async Task<IActionResult> GetWalletTransactionById(
        Guid walletId, 
        Guid transactionId, 
        CancellationToken cancellationToken)
    {
        var query = new GetWalletTransactionByIdQuery(walletId, transactionId);
        var response = await Sender.Send(query, cancellationToken);
        return response.IsSuccess ? Ok(response.Value) : NotFound(response.Error);
    }
    
    [HttpGet("{walletId:guid}/with-transactions")]
    public async Task<IActionResult> GetWalletWithTransactions(
        Guid walletId,
        CancellationToken cancellationToken)
    {
        var query = new GetWalletWithTransactionsByIdQuery(walletId);
        var response = await Sender.Send(query, cancellationToken);
        return response.IsSuccess ? Ok(response.Value) : NotFound(response.Error);
    }
    
    [HttpGet("{walletId:guid}/balance")]
    public async Task<IActionResult> GetWalletBalanceByCurrency(
        Guid walletId,
        [FromQuery] string currencyCode,
        CancellationToken cancellationToken)
    {
        var query = new GetWalletBalanceByCurrencyQuery(walletId, currencyCode);
        var result = await Sender.Send(query, cancellationToken);
        return result.IsFailure ? HandleFailure(result) : Ok(result.Value);
    }

    [HttpGet("{walletId:guid}/loyalty-points")]
    public async Task<IActionResult> GetLoyaltyPointsBalance(
        Guid walletId,
        CancellationToken cancellationToken)
    {
        var query = new GetLoyaltyPointsBalanceQuery(walletId);
        var result = await Sender.Send(query, cancellationToken);
        return result.IsFailure ? HandleFailure(result) : Ok(result.Value);
    }

    #endregion

    #region Post Endpoints

    [HttpPost]
    public async Task<IActionResult> CreateWallet(
        [FromBody] CreateWalletRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateWalletCommand(
            GetUserId(),
            request.Balance,
            request.CurrencyCode);
        var result = await Sender.Send(command, cancellationToken);
        return result.IsFailure ? HandleFailure(result) : Ok(result);
    }

    [HttpPost("{walletId:guid}/transactions")]
    public async Task<IActionResult> AddWalletTransaction(
        Guid walletId, 
        [FromBody] AddWalletTransactionRequest request, 
        CancellationToken cancellationToken)
    {
        var command = new AddWalletTransactionCommand(
            walletId, 
            request.Amount,
            request.Date,
            request.Description);
        var result = await Sender.Send(command, cancellationToken);
        return result.IsFailure ? HandleFailure(result) : Ok(result);
    }
    
    [HttpPost("{walletId:guid}/convert-currency/{newCurrencyCode}")]
    public async Task<IActionResult> ConvertWalletCurrency(
        Guid walletId,
        string newCurrencyCode,
        CancellationToken cancellationToken)
    {
        var command = new ConvertWalletCurrencyCommand(
            walletId,
            newCurrencyCode);
        var result = await Sender.Send(command, cancellationToken);
        return result.IsFailure ? HandleFailure(result) : Ok(result);
    }

    [HttpPost("{walletId:guid}/set-spending-limit")]
    public async Task<IActionResult> SetSpendingLimit(
        Guid walletId,
        [FromBody] SetSpendingLimitRequest request,
        CancellationToken cancellationToken)
    {
        var command = new SetSpendingLimitCommand(
            walletId,
            request.SpendingLimit);
        var result = await Sender.Send(command, cancellationToken);
        return result.IsFailure ? HandleFailure(result) : Ok(result);
    }

    [HttpPost("{walletId:guid}/redeem-loyalty-points")]
    public async Task<IActionResult> RedeemLoyaltyPoints(
        Guid walletId,
        [FromBody] RedeemLoyaltyPointsRequest request,
        CancellationToken cancellationToken)
    {
        var command = new RedeemLoyaltyPointsCommand(
            walletId,
            request.Points);
        var result = await Sender.Send(command, cancellationToken);
        return result.IsFailure ? HandleFailure(result) : Ok(result);
    }

    #endregion

    #region Delete Endpoints

    [HttpDelete("{walletId:guid}/transactions/{transactionId:guid}")]
    public async Task<IActionResult> RemoveWalletTransaction(
        Guid walletId,
        Guid transactionId,
        CancellationToken cancellationToken)
    {
        var command = new RemoveWalletTransactionCommand(walletId, transactionId);
        var result = await Sender.Send(command, cancellationToken);
        return result.IsFailure ? HandleFailure(result) : NoContent();
    }

    #endregion
}
