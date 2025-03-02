using Microsoft.AspNetCore.Mvc;
using PayVerse.Application.Security.Commands.TransferFundsWithDecorators;
using PayVerse.Application.Security.Decorators;
using PayVerse.Domain.Decorators.Security;
using System.Security;

namespace PayVerse.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VirtualAccountsController(
    ISecureOperation<TransferFundsWithDecoratorsCommand, TransferFundsWithDecoratorsResult> transferFundsOperation) : ControllerBase
{
    [HttpPost("transfer")]
    public async Task<ActionResult<TransferFundsWithDecoratorsResult>> TransferFunds(
        [FromBody] TransferFundsWithDecoratorsCommand command,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await transferFundsOperation.ExecuteAsync(command, cancellationToken);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
        catch (ForbiddenAccessException)
        {
            return Forbid();
        }
        catch (RateLimitExceededException ex)
        {
            return StatusCode(429, new { error = ex.Message });
        }
        catch (SecurityException)
        {
            return StatusCode(403, new { error = "Access denied for security reasons" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "An unexpected error occurred", details = ex.Message });
        }
    }
}