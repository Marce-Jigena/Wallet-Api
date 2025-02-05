using Kata.Wallet.Domain;
using Kata.Wallet.Dtos;
using Kata.Wallet.Services;
using Microsoft.AspNetCore.Mvc;

namespace Kata.Wallet.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WalletController : ControllerBase
{
    private readonly IWalletService _walletService;

    public WalletController(IWalletService walletService)
    {
        _walletService = walletService;
    }

    [HttpGet]
    public async Task<ActionResult<List<Domain.Wallet>>> GetAll(Currency? currency, string? userDocument)
    {
        var wallets = await _walletService.GetAllAsync(currency, userDocument);
        return Ok(wallets);
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] WalletDto wallet)
    {
        if (wallet == null)
        {
            return BadRequest();
        }

        await _walletService.CreateAsync(wallet);
        return CreatedAtAction(nameof(GetAll), new { id = wallet.Id }, wallet);
    }
}
