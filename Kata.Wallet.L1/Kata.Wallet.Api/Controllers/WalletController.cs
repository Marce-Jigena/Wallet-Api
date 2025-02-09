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
    public async Task<ActionResult<List<WalletDto>>> GetAll(Currency? currency, string? userDocument)
    {
        var wallets = await _walletService.GetAllAsync(currency, userDocument);
        return Ok(wallets);
    }

    [HttpPut]
    public async Task<ActionResult> NewTransfer(int originWalletId, int destinationWalletId, decimal amount, string description) 
    {
        if (originWalletId == 0 || destinationWalletId == 0) 
        {
            return BadRequest("Debes seleccionar 2 billeteras validas para hacer una transferencia");
        }
        try
        {
            await _walletService.NewTransfer(originWalletId, destinationWalletId, amount, description);
            return Ok();
        }

        catch (DifferentCurrencyException ex)
        {
            return BadRequest(new { ex.Message, ex.ErrorCode, ex.Entity });
        }
        catch (NotEnoughFundsException ex)
        {
            return BadRequest(new { ex.Message, ex.ErrorCode, ex.Entity });
        }
        catch (KeyNotFoundException ex)
        {
            return BadRequest(new { ex.Message });
        }

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
