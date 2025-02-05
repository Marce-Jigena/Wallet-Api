using Kata.Wallet.Domain;
using Kata.Wallet.Dtos;

namespace Kata.Wallet.Services;

public interface IWalletService
{
    Task<List<Domain.Wallet>> GetAllAsync(Currency? currency, string? userDocument);
    Task CreateAsync(WalletDto walletDto);
}
