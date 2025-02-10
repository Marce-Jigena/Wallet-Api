using Kata.Wallet.Domain;
using Kata.Wallet.Dtos;
using Kata.Wallet.Services;
using Microsoft.AspNetCore.Mvc;

namespace Kata.Wallet.Api.Controllers
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

        [HttpGet]
        public async Task<ActionResult<List<TransactionDto>>> GetTransactionsAsync(int walletId)
        {
            var transactions = await _transactionService.GetTransactionsAsync(walletId);

            if (transactions == null)
            {
                return BadRequest();
            }

            return Ok(transactions);
        }
    }
}
