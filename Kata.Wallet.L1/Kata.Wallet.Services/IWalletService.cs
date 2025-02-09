using Kata.Wallet.Domain;
using Kata.Wallet.Dtos;

namespace Kata.Wallet.Services;

public interface IWalletService
{
    Task<List<WalletDto>> GetAllAsync(Currency? currency, string? userDocument);
    Task<List<Transaction>> GetTransactionsAsync(int walletId);
    Task CreateAsync(WalletDto walletDto);
    Task NewTransfer(int originWalletId, int destinationWalletId, decimal amount, string description);
}
