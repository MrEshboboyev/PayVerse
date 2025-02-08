using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayVerse.Application.VirtualAccounts.Commands.AddTransaction;
using PayVerse.Application.VirtualAccounts.Commands.CreateVirtualAccount;
using PayVerse.Application.VirtualAccounts.Commands.TransferFunds;
using PayVerse.Application.VirtualAccounts.Queries.GetTransactionById;
using PayVerse.Application.VirtualAccounts.Queries.GetTransactions;
using PayVerse.Application.VirtualAccounts.Queries.GetVirtualAccountById;
using PayVerse.Application.VirtualAccounts.Queries.GetVirtualAccountsByUserId;
using PayVerse.Application.VirtualAccounts.Queries.GetVirtualAccountWithTransactionsById;
using PayVerse.Presentation.Abstractions;
using PayVerse.Presentation.Contracts.VirtualAccounts;

namespace PayVerse.Presentation.Controllers;

[Route("api/virtual-accounts")]
[Authorize]
public sealed class VirtualAccountController(ISender sender) : ApiController(sender)
{
    #region Get Endpoints

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetVirtualAccountById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var query = new GetVirtualAccountByIdQuery(id);
        var response = await Sender.Send(query, cancellationToken);
        return response.IsSuccess ? Ok(response.Value) : NotFound(response.Error);
    }

    [HttpGet("user")]
    public async Task<IActionResult> GetVirtualAccounts(
        CancellationToken cancellationToken)
    {
        var query = new GetVirtualAccountsByUserIdQuery(GetUserId());
        var response = await Sender.Send(query, cancellationToken);
        return response.IsSuccess ? Ok(response.Value) : NotFound(response.Error);
    }

    [HttpGet("{virtualAccountId:guid}/transactions")]
    public async Task<IActionResult> GetTransactions(
        Guid virtualAccountId,
        CancellationToken cancellationToken)
    {
        var query = new GetTransactionsQuery(virtualAccountId);
        var response = await Sender.Send(query, cancellationToken);
        return response.IsSuccess ? Ok(response.Value) : NotFound(response.Error);
    }
    
    [HttpGet("{virtualAccountId:guid}/transactions/{transactionId:guid}")]
    public async Task<IActionResult> GetTransactionById(
        Guid virtualAccountId,
        Guid transactionId,
        CancellationToken cancellationToken)
    {
        var query = new GetTransactionByIdQuery(virtualAccountId, transactionId);
        var response = await Sender.Send(query, cancellationToken);
        return response.IsSuccess ? Ok(response.Value) : NotFound(response.Error);
    }

    [HttpGet("{virtualAccountId:guid}/with-transactions")]
    public async Task<IActionResult> GetVirtualAccountWithTransactionsById(
        Guid virtualAccountId,
        CancellationToken cancellationToken)
    {
        var query = new GetVirtualAccountWithTransactionsByIdQuery(virtualAccountId);
        var response = await Sender.Send(query, cancellationToken);
        return response.IsSuccess ? Ok(response.Value) : NotFound(response.Error);
    }

    #endregion

    #region Post Endpoints

    [HttpPost]
    public async Task<IActionResult> CreateVirtualAccount(
        [FromBody] CreateVirtualAccountRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateVirtualAccountCommand(
            request.AccountNumber,
            request.CurrencyCode,
            request.Balance,
            GetUserId());
        var result = await Sender.Send(command, cancellationToken);
        return result.IsFailure ? HandleFailure(result) : Ok(result);
    }

    [HttpPost("{virtualAccountId:guid}/transactions")]
    public async Task<IActionResult> AddTransaction(
        Guid virtualAccountId,
        [FromBody] AddTransactionRequest request, 
        CancellationToken cancellationToken)
    {
        var command = new AddTransactionCommand(
            virtualAccountId, 
            request.Amount,
            request.Date,
            request.Description);
        var result = await Sender.Send(command, cancellationToken);
        return result.IsFailure ? HandleFailure(result) : Ok(result);
    }
    
    [HttpPost("transfer-funds")]
    public async Task<IActionResult> TransferFunds(
        [FromBody] TransferFundsRequest request,
        CancellationToken cancellationToken)
    {
        var command = new TransferFundsCommand(
            request.FromAccountId,
            request.ToAccountId,
            request.Amount);
        var result = await Sender.Send(command, cancellationToken);
        return result.IsFailure ? HandleFailure(result) : Ok(result);
    }

    #endregion
}
