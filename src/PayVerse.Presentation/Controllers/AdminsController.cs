using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayVerse.Application.Invoices.Queries.GetAllInvoices;
using PayVerse.Application.Invoices.Queries.GetInvoicesByDateRange;
using PayVerse.Application.Invoices.Queries.GetInvoicesByStatus;
using PayVerse.Application.Invoices.Queries.GetInvoicesByUserId;
using PayVerse.Application.Invoices.Queries.GetOverdueInvoices;
using PayVerse.Application.Invoices.Queries.GetTotalRevenueByUser;
using PayVerse.Application.Notifications.Commands.CreateNotification;
using PayVerse.Application.Notifications.Commands.SendNotification;
using PayVerse.Application.Notifications.Commands.UpdateNotificationMessage;
using PayVerse.Application.Payments.Commands.RefundPayment;
using PayVerse.Application.Payments.Commands.RetryFailedPayment;
using PayVerse.Application.Payments.Queries.GetAllPayments;
using PayVerse.Application.Payments.Queries.GetPaymentsByDateRange;
using PayVerse.Application.Payments.Queries.GetPaymentsByUserId;
using PayVerse.Application.Reports.Queries.GetAllReports;
using PayVerse.Application.Reports.Queries.GetCompositeFinancialReportsByPeriod;
using PayVerse.Application.Reports.Queries.GetCompositeFinancialReportsByUser;
using PayVerse.Application.Users.Commands.AssignRoleToUser;
using PayVerse.Application.Users.Commands.BlockUser;
using PayVerse.Application.Users.Commands.ResetPassword;
using PayVerse.Application.Users.Commands.RevokeRoleFromUser;
using PayVerse.Application.Users.Commands.UnblockUser;
using PayVerse.Application.Users.Queries.GetAllRoles;
using PayVerse.Application.Users.Queries.GetAllUsers;
using PayVerse.Application.Users.Queries.GetUserById;
using PayVerse.Application.Users.Queries.GetUserRoles;
using PayVerse.Application.Users.Queries.GetUserWithRolesById;
using PayVerse.Application.Users.Queries.SearchUsers;
using PayVerse.Application.VirtualAccounts.Commands.CloseVirtualAccount;
using PayVerse.Application.VirtualAccounts.Commands.FreezeVirtualAccount;
using PayVerse.Application.VirtualAccounts.Commands.SetOverdraftLimit;
using PayVerse.Application.VirtualAccounts.Commands.UnfreezeVirtualAccount;
using PayVerse.Application.VirtualAccounts.Queries.GetAllVirtualAccounts;
using PayVerse.Application.VirtualAccounts.Queries.GetVirtualAccountsByUserId;
using PayVerse.Application.Wallets.Queries.GetAllWallets;
using PayVerse.Application.Wallets.Queries.GetWalletsByUserId;
using PayVerse.Domain.Enums.Invoices;
using PayVerse.Presentation.Abstractions;
using PayVerse.Presentation.Contracts.Admins;
using PayVerse.Presentation.Contracts.Payments;
using PayVerse.Presentation.Contracts.Users;

namespace PayVerse.Presentation.Controllers;

[Route("api/admins")]
[Authorize(Roles = "Admin")]
public sealed class AdminsController(ISender sender) : ApiController(sender)
{
    #region Users

    #region Get endpoints

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

    [HttpGet("users/{userId:guid}/roles")]
    public async Task<IActionResult> GetUserRoles(
        Guid userId,
        CancellationToken cancellationToken)
    {
        var query = new GetUserRolesQuery(userId);
        var response = await Sender.Send(query, cancellationToken);
        return response.IsSuccess ? Ok(response.Value) : NotFound(response.Error);
    }

    [HttpGet("search-users")]
    public async Task<IActionResult> GetUserRoles(
        [FromBody] SearchUsersRequest request,
        CancellationToken cancellationToken)
    {
        var query = new SearchUsersQuery(
            request.Email,
            request.Name,
            request.RoleId);
        var response = await Sender.Send(query, cancellationToken);
        return response.IsSuccess ? Ok(response.Value) : NotFound(response.Error);
    }

    #endregion

    #region Post endpoints

    [HttpPost("users/reset-password")]
    public async Task<IActionResult> ChangePassword(
        [FromBody] ResetPasswordRequest request,
        CancellationToken cancellationToken)
    {
        var command = new ResetPasswordCommand(
            request.UserId,
            request.NewPassword);
        var result = await Sender.Send(command, cancellationToken);
        return result.IsFailure ? HandleFailure(result) : Ok(result);
    }

    [HttpPost("users/block/{userId:guid}")]
    public async Task<IActionResult> BlockUser(
        Guid userId,
        CancellationToken cancellationToken)
    {
        var command = new BlockUserCommand(userId);
        var result = await Sender.Send(command, cancellationToken);
        return result.IsFailure ? HandleFailure(result) : Ok(result);
    }

    [HttpPost("users/unblock/{userId:guid}")]
    public async Task<IActionResult> UnblockUser(
        Guid userId,
        CancellationToken cancellationToken)
    {
        var command = new UnblockUserCommand(userId);
        var result = await Sender.Send(command, cancellationToken);
        return result.IsFailure ? HandleFailure(result) : Ok(result);
    }

    [HttpPost("users/{userId:guid}/assign-role/{roleId:int}")]
    public async Task<IActionResult> AssignRoleToUser(
        Guid userId,
        int roleId,
        CancellationToken cancellationToken)
    {
        var command = new AssignRoleToUserCommand(
            userId,
            roleId);
        var result = await Sender.Send(command, cancellationToken);
        return result.IsFailure ? HandleFailure(result) : Ok(result);
    }

    [HttpPost("users/{userId:guid}/revoke-role/{roleId:int}")]
    public async Task<IActionResult> RevokeRoleFromUser(
        Guid userId,
        int roleId,
        CancellationToken cancellationToken)
    {
        var command = new RevokeRoleFromUserCommand(
            userId,
            roleId);
        var result = await Sender.Send(command, cancellationToken);
        return result.IsFailure ? HandleFailure(result) : Ok(result);
    }

    #endregion

    #endregion

    #region Invoices

    #region Get endpoints

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

    [HttpGet("invoices/overdue")]
    public async Task<IActionResult> GetOverdueInvoices(
        CancellationToken cancellationToken)
    {
        var query = new GetOverdueInvoicesQuery();
        var result = await Sender.Send(query, cancellationToken);
        return result.IsFailure ? HandleFailure(result) : Ok(result.Value);
    }

    [HttpGet("invoices/status/{status}")]
    public async Task<IActionResult> GetInvoicesByStatus(
        InvoiceStatus status,
        CancellationToken cancellationToken)
    {
        var query = new GetInvoicesByStatusQuery(status);
        var result = await Sender.Send(query, cancellationToken);
        return result.IsFailure ? HandleFailure(result) : Ok(result.Value);
    }

    [HttpGet("invoices/date-range/{startDate:datetime}/{endDate:datetime}")]
    public async Task<IActionResult> GetInvoicesByDateRange(
        DateTime startDate,
        DateTime endDate,
        CancellationToken cancellationToken)
    {
        var query = new GetInvoicesByDateRangeQuery(startDate, endDate);
        var result = await Sender.Send(query, cancellationToken);
        return result.IsFailure ? HandleFailure(result) : Ok(result.Value);
    }

    [HttpGet("invoices/{userId:guid}/total-revenue")]
    public async Task<IActionResult> GetTotalRevenueByUser(
        Guid userId,
        CancellationToken cancellationToken)
    {
        var query = new GetTotalRevenueByUserQuery(userId);
        var result = await Sender.Send(query, cancellationToken);
        return result.IsFailure ? HandleFailure(result) : Ok(result.Value);
    }

    #endregion

    #endregion

    #region VirtualAccounts

    #region Get endpoints

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

    #region Post endpoints

    [HttpPost("virtual-accounts/{accountId:guid}/close")]
    public async Task<IActionResult> CloseVirtualAccount(
        Guid accountId,
        CancellationToken cancellationToken)
    {
        var command = new CloseVirtualAccountCommand(accountId);
        var result = await Sender.Send(command, cancellationToken);
        return result.IsFailure ? HandleFailure(result) : Ok(result);
    }

    [HttpPost("virtual-accounts/{accountId:guid}/freeze")]
    public async Task<IActionResult> FreezeVirtualAccount(
        Guid accountId,
        CancellationToken cancellationToken)
    {
        var command = new FreezeVirtualAccountCommand(accountId);
        var result = await Sender.Send(command, cancellationToken);
        return result.IsFailure ? HandleFailure(result) : Ok(result);
    }

    [HttpPost("virtual-accounts/{accountId:guid}/unfreeze")]
    public async Task<IActionResult> UnfreezeVirtualAccount(
        Guid accountId,
        CancellationToken cancellationToken)
    {
        var command = new UnfreezeVirtualAccountCommand(accountId);
        var result = await Sender.Send(command, cancellationToken);
        return result.IsFailure ? HandleFailure(result) : Ok(result);
    }

    [HttpPost("virtual-accounts/set-overdraft-limit")]
    public async Task<IActionResult> SetOverdraftLimit(
        [FromBody] SetOverdraftLimitRequest request,
        CancellationToken cancellationToken)
    {
        var command = new SetOverdraftLimitCommand(
            request.AccountId,
            request.OverdraftLimit);
        var result = await Sender.Send(command, cancellationToken);
        return result.IsFailure ? HandleFailure(result) : Ok(result);
    }

    #endregion

    #endregion

    #region Payments

    #region Get endpoints

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

    [HttpGet("payments/date-range/{startDate:datetime}/{endDate:datetime}")]
    public async Task<IActionResult> GetPaymentsByDateRange(
        DateTime startDate,
        DateTime endDate,
        CancellationToken cancellationToken)
    {
        var query = new GetPaymentsByDateRangeQuery(startDate, endDate);
        var result = await Sender.Send(query, cancellationToken);
        return result.IsFailure ? HandleFailure(result) : Ok(result.Value);
    }

    #endregion

    #region Patch endpoints

    [HttpPatch("{paymentId:guid}/refund")]
    public async Task<IActionResult> RefundPayment(
        Guid paymentId,
        [FromBody] RefundPaymentRequest request,
        CancellationToken cancellationToken)
    {
        var command = new RefundPaymentCommand(
            paymentId,
            request.RefundTransactionId,
            request.Reason);
        var result = await Sender.Send(command, cancellationToken);
        return result.IsFailure ? HandleFailure(result) : Ok(result);
    }

    [HttpPatch("{paymentId:guid}/retry")]
    public async Task<IActionResult> RetryFailedPayment(
        Guid paymentId,
        CancellationToken cancellationToken)
    {
        var command = new RetryFailedPaymentCommand(paymentId);
        var result = await Sender.Send(command, cancellationToken);
        return result.IsFailure ? HandleFailure(result) : Ok(result);
    }

    #endregion

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

    #region Notifications

    #region Post endpoints

    [HttpPost("notifications")]
    public async Task<IActionResult> CreateNotification(
        [FromBody] CreateNotificationRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateNotificationCommand(
            request.Message,
            request.Type,
            request.Priority,
            request.UserId,
            request.DeliveryMethod);
        var result = await Sender.Send(command, cancellationToken);
        return result.IsFailure ? HandleFailure(result) : Ok(result);
    }

    #endregion

    #region Put endpoints

    [HttpPut("notifications/{notificationId:guid}/send")]
    public async Task<IActionResult> SendNotification(
        Guid notificationId,
        CancellationToken cancellationToken)
    {
        var command = new SendNotificationCommand(notificationId);
        var result = await Sender.Send(command, cancellationToken);
        return result.IsFailure ? HandleFailure(result) : Ok(result);
    }

    [HttpPut("notifications/{notificationId:guid}")]
    public async Task<IActionResult> UpdateNotification(
        Guid notificationId,
        [FromBody] UpdateNotificationRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateNotificationMessageCommand(
            notificationId,
            request.NewMessage);
        var result = await Sender.Send(command, cancellationToken);
        return result.IsFailure ? HandleFailure(result) : Ok(result);
    }

    #endregion

    #endregion

    #region Reports

    #region Get endpoints

    [HttpGet("reports")]
    public async Task<IActionResult> GetAllReports(CancellationToken cancellationToken)
    {
        var query = new GetAllReportsQuery();
        var response = await Sender.Send(query, cancellationToken);
        return response.IsSuccess ? Ok(response.Value) : NotFound(response.Error);
    }

    // fix this -> coming soon
    [HttpGet("reports/{startDate}/{endDate}")]
    public async Task<IActionResult> GetReportsByPeriod(
        string startDate,
        string endDate,
        CancellationToken cancellationToken)
    {
        if (!DateOnly.TryParse(startDate, out var startDateOnly))
        {
            return BadRequest("Invalid start date format.");
        }

        if (!DateOnly.TryParse(endDate, out var endDateOnly))
        {
            return BadRequest("Invalid end date format.");
        }

        var query = new GetCompositeFinancialReportsByPeriodQuery(startDateOnly, endDateOnly);
        var response = await Sender.Send(query, cancellationToken);
        return response.IsSuccess ? Ok(response.Value) : NotFound(response.Error);
    }

    [HttpGet("reports/user/{userId:guid}")]
    public async Task<IActionResult> GetUserReports(
        Guid userId,
        CancellationToken cancellationToken)
    {
        var query = new GetCompositeFinancialReportsByUserQuery(userId);
        var response = await Sender.Send(query, cancellationToken);
        return response.IsSuccess ? Ok(response.Value) : NotFound(response.Error);
    }

    #endregion

    #endregion
}