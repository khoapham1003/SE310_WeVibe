using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WeVibe.Core.Contracts.Transaction;
using WeVibe.Core.Services.Abstractions.Features;

namespace WeVibe.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionController : Controller
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }
        [HttpPost]
        [SwaggerOperation(Summary = "Create a transaction", Description = "Add a new transaction.")]
        [SwaggerResponse(201, "Transaction created", typeof(TransactionDto))]
        [SwaggerResponse(400, "Transaction creation failed")]
        public async Task<IActionResult> CreateTransaction([FromBody] CreateTransactionDto createTransactionDto)
        {
            try
            {
                var transaction = await _transactionService.CreateTransactionAsync(createTransactionDto);

                return Ok(transaction);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Status = "Fail", Message = $"Transaction creation failed: {ex.Message}" });
            }
        }
    }
}
