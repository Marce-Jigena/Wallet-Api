using Kata.Wallet.Domain;
namespace Kata.Wallet.Database;

public interface IWalletRepository
{
    Task<List<Domain.Wallet>> GetAllAsync(Currency? currency, string? userDocument);
    Task<Domain.Wallet> GetByIdAsync(int id);
    Task AddAsync(Domain.Wallet wallet);
    Task UpdateAsync(Domain.Wallet wallet);
    Task DeleteAsync(int id);
}
