using Kata.Wallet.Domain;

namespace Kata.Wallet.Database;

public interface ITransactionRepository
{
    Task<List<Transaction>> GetAllAsync(int walletId);
    Task<Transaction> GetByIdAsync(int id);
    Task AddAsync(Transaction transaction);
    Task UpdateAsync(Transaction transaction);
    Task DeleteAsync(int id);
}

