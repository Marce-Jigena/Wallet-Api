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
                return await _context.Wallets.Where(w => w.Currency == currency && w.UserDocument == userDocument).Include(t => t.IncomingTransactions).Include(t => t.OutgoingTransactions).ToListAsync();

            case (true, false):
                return await _context.Wallets.Where(w => w.Currency == currency).Include(t => t.IncomingTransactions).Include(t => t.OutgoingTransactions).ToListAsync();
            
            case (false, true):
                return await _context.Wallets.Where(w => w.UserDocument == userDocument).Include(t => t.IncomingTransactions).Include(t => t.OutgoingTransactions).ToListAsync();

            default:
                return await _context.Wallets.Include(t => t.IncomingTransactions).Include(t => t.OutgoingTransactions).ToListAsync();
        }
    }

    public async Task NewTransfer(Domain.Wallet originWallet, Domain.Wallet destinationWallet, decimal amount, string description)
    {

        if (!(originWallet == null || destinationWallet == null)) 
        {
            if (originWallet.Balance >= amount)
            {
                originWallet.Balance -= amount;
                if (originWallet.OutgoingTransactions == null)
                {
                    originWallet.OutgoingTransactions = new List<Transaction>();
                }

                var outgoingTransaction = new Transaction
                {
                    Amount = amount,
                    Date = DateTime.UtcNow,
                    Description = description,
                    WalletOutgoing = destinationWallet
                };

                _context.Transactions.Add(outgoingTransaction);

                originWallet.OutgoingTransactions.Add(outgoingTransaction);

                destinationWallet.Balance += amount;
                if (destinationWallet.IncomingTransactions == null)
                {
                    destinationWallet.IncomingTransactions = new List<Transaction>();

                }

                var incomingTransaction = new Transaction
                {
                    Amount = amount,
                    Date = DateTime.UtcNow,
                    Description = description,
                    WalletIncoming= originWallet
                };
                _context.Transactions.Add(incomingTransaction);

                destinationWallet.IncomingTransactions.Add(incomingTransaction);
            }
            else throw new NotEnoughFundsException(409, "Wallet", "No hay suficientes fondos para realizar la transaccion");
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
        _context.Update(wallet);
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
