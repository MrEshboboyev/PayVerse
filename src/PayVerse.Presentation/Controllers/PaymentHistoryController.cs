using MediatR;
using Microsoft.AspNetCore.Mvc;
using PayVerse.Application.Mementos.Payments;
using PayVerse.Presentation.Abstractions;

namespace PayVerse.Presentation.Controllers;

[ApiController]
[Route("api/payments/{paymentId}/history")]
public class PaymentHistoryController(
    ISender sender,
    IPaymentHistoryService historyService) : ApiController(sender)
{
    [HttpGet]
    public async Task<IActionResult> GetPaymentHistory(Guid paymentId)
    {
        var result = await historyService.GetPaymentHistoryAsync(paymentId);

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return BadRequest(result.Error);
    }

    [HttpPost("restore/{version}")]
    public async Task<IActionResult> RestorePaymentState(Guid paymentId, int version)
    {
        var result = await historyService.RestorePaymentStateAsync(paymentId, version);

        if (result.IsSuccess)
        {
            // Return DTO transformed from the domain object
            return Ok(new
            {
                id = result.Value.Id,
                status = result.Value.Status.ToString(),
                amount = result.Value.Amount.Value,
                currency = "USD",
                restoredAt = DateTime.UtcNow
            });
        }

        return BadRequest(result.Error);
    }
}