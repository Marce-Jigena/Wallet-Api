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

    public async Task<List<Domain.Wallet>> GetAllAsync(Currency? currency, string? userDocument)
    {
        return await _walletRepository.GetAllAsync(currency, userDocument);
    }

    public async Task NewTransfer(int originWalletId, int destinationWalletId, decimal amount, string description) 
    {
        await _walletRepository.NewTransfer(originWalletId, destinationWalletId, amount, description);
    }

    public async Task CreateAsync(WalletDto walletDto)
    {
        var wallet = _mapper.Map<Domain.Wallet>(walletDto);
        await _walletRepository.AddAsync(wallet);
    }
}

