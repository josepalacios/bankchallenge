
using Microsoft.AspNetCore.Mvc;
using TransactionService.Application.Commands;
using TransactionService.Application.Interfaces;

namespace TransactionService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionController : ControllerBase
{
    private readonly ITransactionService _transactionService;

    public TransactionController(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateTransaction([FromBody] SubmitTransactionCommand command)
    {
        var transactionId = await _transactionService.SubmitTransactionAsync(command);
        return Ok(new { TransactionExternalId = transactionId, CreatedAt = DateTime.UtcNow });
    }

    [HttpGet("by-account-and-date")]
    public async Task<IActionResult> GetBySourceAccountAndDate([FromQuery] Guid sourceAccountId, [FromQuery] DateTime date)
    {
        if (sourceAccountId == Guid.Empty)
            return BadRequest("Invalid source account ID.");

        if (date == default)
            return BadRequest("Invalid date.");

        var transactions = await _transactionService.GetBySourceAccountAndDateAsync(sourceAccountId, date);

        if (transactions == null || !transactions.Any())
            return NotFound("No transactions found for the given account and date.");

        return Ok(transactions);
    }
}
