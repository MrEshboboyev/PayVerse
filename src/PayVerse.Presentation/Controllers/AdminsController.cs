using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayVerse.Application.Invoices.Queries.GetAllInvoices;
using PayVerse.Application.Invoices.Queries.GetInvoicesByUserId;
using PayVerse.Application.Payments.Queries.GetAllPayments;
using PayVerse.Application.Payments.Queries.GetPaymentsByUserId;
using PayVerse.Application.Users.Queries.GetAllRoles;
using PayVerse.Application.Users.Queries.GetAllUsers;
using PayVerse.Application.Users.Queries.GetUserById;
using PayVerse.Application.Users.Queries.GetUserWithRolesById;
using PayVerse.Application.VirtualAccounts.Queries.GetAllVirtualAccounts;
using PayVerse.Application.VirtualAccounts.Queries.GetVirtualAccountsByUserId;
using PayVerse.Application.Wallets.Queries.GetAllWallets;
using PayVerse.Application.Wallets.Queries.GetWalletsByUserId;
using PayVerse.Presentation.Abstractions;

namespace PayVerse.Presentation.Controllers;

[Route("api/admins")]
[Authorize(Roles = "Admin")]
public sealed class AdminsController(ISender sender) : ApiController(sender)
{
    #region Get Endpoints
    
    #region Users
    
    [HttpGet("users")]
    public async Task<IActionResult> GetAllUsers(CancellationToken cancellationToken)
    {
        var query = new GetAllUsersQuery();
        var response = await Sender.Send(query, cancellationToken);
        return response.IsSuccess ? Ok(response.Value) : NotFound(response.Error);
    }
    
    [HttpGet("users/{userId:guid}")]
    public async Task<IActionResult> GetUserById(
        Guid userId,
        CancellationToken cancellationToken)
    {
        var query = new GetUserByIdQuery(userId);
        var response = await Sender.Send(query, cancellationToken);
        return response.IsSuccess ? Ok(response.Value) : NotFound(response.Error);
    }
    
    
    [HttpGet("users/{userId:guid}/with-roles")]
    public async Task<IActionResult> GetUserWithRolesById(
        Guid userId,
        CancellationToken cancellationToken)
    {
        var query = new GetUserWithRolesByIdQuery(userId);
        var response = await Sender.Send(query, cancellationToken);
        return response.IsSuccess ? Ok(response.Value) : NotFound(response.Error);
    }
    
    // Roles
    [HttpGet("roles")]
    public async Task<IActionResult> GetAllRoles(
        CancellationToken cancellationToken)
    {
        var query = new GetAllRolesQuery();
        var response = await Sender.Send(query, cancellationToken);
        return response.IsSuccess ? Ok(response.Value) : NotFound(response.Error);
    }
    
    #endregion

    #region Invoices
    
    [HttpGet("invoices")]
    public async Task<IActionResult> GetAllInvoices(CancellationToken cancellationToken)
    {
        var query = new GetAllInvoicesQuery();
        var response = await Sender.Send(query, cancellationToken);
        return response.IsSuccess ? Ok(response.Value) : NotFound(response.Error);
    }
    
    [HttpGet("invoices/user/{userId:guid}")]
    public async Task<IActionResult> GetInvoicesByUserId(
        Guid userId,
        CancellationToken cancellationToken)
    {
        var query = new GetInvoicesByUserIdQuery(userId);
        var response = await Sender.Send(query, cancellationToken);
        return response.IsSuccess ? Ok(response.Value) : NotFound(response.Error);
    }
    
    #endregion
    
    #region VirtualAccounts
    
    [HttpGet("virtual_accounts")]
    public async Task<IActionResult> GetAllVirtualAccounts(CancellationToken cancellationToken)
    {
        var query = new GetAllVirtualAccountsQuery();
        var response = await Sender.Send(query, cancellationToken);
        return response.IsSuccess ? Ok(response.Value) : NotFound(response.Error);
    }
    
    [HttpGet("virtual_accounts/user/{userId:guid}")]
    public async Task<IActionResult> GetVirtualAccountsByUserId(
        Guid userId,
        CancellationToken cancellationToken)
    {
        var query = new GetVirtualAccountsByUserIdQuery(userId);
        var response = await Sender.Send(query, cancellationToken);
        return response.IsSuccess ? Ok(response.Value) : NotFound(response.Error);
    }
    
    #endregion
    
    #region Payments
    
    [HttpGet("payments")]
    public async Task<IActionResult> GetAllPayments(CancellationToken cancellationToken)
    {
        var query = new GetAllPaymentsQuery();
        var response = await Sender.Send(query, cancellationToken);
        return response.IsSuccess ? Ok(response.Value) : NotFound(response.Error);
    }
    
    [HttpGet("payments/user/{userId:guid}")]
    public async Task<IActionResult> GetPaymentsByUserId(
        Guid userId,
        CancellationToken cancellationToken)
    {
        var query = new GetPaymentsByUserIdQuery(userId);
        var response = await Sender.Send(query, cancellationToken);
        return response.IsSuccess ? Ok(response.Value) : NotFound(response.Error);
    }
    
    #endregion
    
    #region Wallets
    
    [HttpGet("wallets")]
    public async Task<IActionResult> GetAllWallets(CancellationToken cancellationToken)
    {
        var query = new GetAllWalletsQuery();
        var response = await Sender.Send(query, cancellationToken);
        return response.IsSuccess ? Ok(response.Value) : NotFound(response.Error);
    }
    
    [HttpGet("wallets/user/{userId:guid}")]
    public async Task<IActionResult> GetWalletsByUserId(
        Guid userId,
        CancellationToken cancellationToken)
    {
        var query = new GetWalletsByUserIdQuery(userId);
        var response = await Sender.Send(query, cancellationToken);
        return response.IsSuccess ? Ok(response.Value) : NotFound(response.Error);
    }
    
    #endregion
    
    #endregion
}