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

    public async Task NewTransfer(int originWalletId, int destinationWalletId, decimal amount, string description)
    {
        var originWallet = _context.Wallets.FirstOrDefault(ow => ow.Id == originWalletId);

        var destinationWallet = _context.Wallets.FirstOrDefault(dw => dw.Id == destinationWalletId);

        if (!(originWallet == null || destinationWallet == null)) 
        {
            if (!(originWallet.Balance < amount))
            {
                originWallet.Balance -= amount;
                if (originWallet.OutgoingTransactions == null) 
                {
                    originWallet.OutgoingTransactions = new List<Transaction>();
               
                }

                originWallet.OutgoingTransactions.Add(new Transaction
                {
                    Amount = amount,
                    Date = DateTime.UtcNow,
                    Description = description,
                    WalletOutgoing = destinationWallet
                });

                destinationWallet.Balance += amount;
                if (destinationWallet.IncomingTransactions == null)
                {
                    destinationWallet.IncomingTransactions = new List<Transaction>();

                }

                destinationWallet.IncomingTransactions.Add(new Transaction
                {
                    Amount = amount,
                    Date = DateTime.UtcNow,
                    Description = description,
                    WalletIncoming = originWallet
                });

                _context.Wallets.Update(originWallet);
                _context.Wallets.Update(originWallet);
            }
        }

        await _context.SaveChangesAsync();
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
