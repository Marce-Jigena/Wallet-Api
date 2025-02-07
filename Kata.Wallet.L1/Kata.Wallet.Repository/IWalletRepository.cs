using Kata.Wallet.Domain;
namespace Kata.Wallet.Database;

public interface IWalletRepository
{
    Task<List<Domain.Wallet>> GetAllAsync(Currency? currency, string? userDocument);
    Task<List<Transaction>> GetTransactionsAsync(Domain.Wallet wallet);
    Task<Domain.Wallet> GetByIdAsync(int id);
    Task NewTransfer(Domain.Wallet originWallet, Domain.Wallet destinationWalletId, decimal amount, string description);
    Task AddAsync(Domain.Wallet wallet);
    Task UpdateAsync(Domain.Wallet wallet);
    Task DeleteAsync(int id);
}
