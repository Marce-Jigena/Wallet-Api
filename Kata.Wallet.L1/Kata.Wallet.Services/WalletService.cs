using AutoMapper;
using Kata.Wallet.Database;
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

    public async Task<List<Domain.Wallet>> GetAllAsync()
    {
        return await _walletRepository.GetAllAsync();
    }

    public async Task CreateAsync(WalletDto walletDto)
    {
        var wallet = _mapper.Map<Domain.Wallet>(walletDto);
        await _walletRepository.AddAsync(wallet);
    }
}

