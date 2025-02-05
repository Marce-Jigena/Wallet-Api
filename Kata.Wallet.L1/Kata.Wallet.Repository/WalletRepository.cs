using Kata.Wallet.Domain;
using Microsoft.EntityFrameworkCore;

namespace Kata.Wallet.Database;

public class WalletRepository : IWalletRepository
{
    private readonly DataContext _context;

    public WalletRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<List<Domain.Wallet>> GetAllAsync(Currency? currency, string? userDocument)
    {
        switch ((currency.HasValue, userDocument != null))
        {
            case (true, true):
                return await _context.Wallets.Where(w => w.Currency == currency && w.UserDocument == userDocument).ToListAsync();

            case (true, false):
                return await _context.Wallets.Where(w => w.Currency == currency).ToListAsync();
            
            case (false, true):
                return await _context.Wallets.Where(w => w.UserDocument == userDocument).ToListAsync();

            default:
                return await _context.Wallets.ToListAsync();
        }
    }

    public async Task<Domain.Wallet> GetByIdAsync(int id)
    {
        return await _context.Wallets.FindAsync(id);
    }

    public async Task AddAsync(Domain.Wallet wallet)
    {
        await _context.Wallets.AddAsync(wallet);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Domain.Wallet wallet)
    {
        _context.Wallets.Update(wallet);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var wallet = await _context.Wallets.FindAsync(id);
        if (wallet != null)
        {
            _context.Wallets.Remove(wallet);
            await _context.SaveChangesAsync();
        }
    }
}
