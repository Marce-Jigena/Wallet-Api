using Kata.Wallet.Dtos;

namespace Kata.Wallet.Services;

public interface IWalletService
{
    Task<List<Domain.Wallet>> GetAllAsync();
    Task CreateAsync(WalletDto walletDto);
}
