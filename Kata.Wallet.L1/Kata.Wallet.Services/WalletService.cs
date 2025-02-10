using AutoMapper;
using Kata.Wallet.Database;
using Kata.Wallet.Domain;
using Kata.Wallet.Dtos;

namespace Kata.Wallet.Services;

public class WalletService : IWalletService
{
    private readonly IWalletRepository _walletRepository;
    public readonly IMapper _mapper;

    public WalletService(
        IMapper mapper, 
        IWalletRepository walletRepository)
    {
        _mapper = mapper;
        _walletRepository = walletRepository;
    }

    public async Task<List<WalletDto>> GetAllAsync(Currency? currency, string? userDocument)
    {
        var wallet = await _walletRepository.GetAllAsync(currency, userDocument);
        var walletDto =  _mapper.Map<List<WalletDto>>(wallet);
        return walletDto;
    }

    public async Task NewTransfer(int originWalletId, int destinationWalletId, decimal amount, string description) 
    {
        var originWallet = await _walletRepository.GetByIdAsync(originWalletId);
        var destinationWallet = await _walletRepository.GetByIdAsync(destinationWalletId);

        //Check de que exista la wallet
        if (Exists(originWallet) && Exists(destinationWallet))
        {
            if (originWallet.Currency != destinationWallet.Currency)
            {
                throw new DifferentCurrencyException(409, "Wallet", "La moneda de ambas cuentas debe ser la misma.");
            }
            else
            {
                //Se crea transferencia y transaccion.
                await _walletRepository.NewTransfer(originWallet, destinationWallet, amount, description);
                await _walletRepository.UpdateAsync(originWallet);
                await _walletRepository.UpdateAsync(destinationWallet);
            }
        }
        else
        {
            throw new KeyNotFoundException("Una o mas wallets no fueron encontradas.");
        }
    }

    public async Task CreateAsync(WalletDto walletDto)
    {
        var wallet = _mapper.Map<Domain.Wallet>(walletDto);
        await _walletRepository.AddAsync(wallet);
    }

    public bool Exists(Domain.Wallet wallet) 
    {
        if (wallet == null) return false;
        else return true;
    }
}

