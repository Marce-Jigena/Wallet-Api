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
                return await _context.Wallets.Where(w => w.Currency == currency && w.UserDocument == userDocument).Include(t => t.IncomingTransactions).ToListAsync();

            case (true, false):
                return await _context.Wallets.Where(w => w.Currency == currency).Include(t => t.IncomingTransactions).ToListAsync();
            
            case (false, true):
                return await _context.Wallets.Where(w => w.UserDocument == userDocument).Include(t => t.IncomingTransactions).ToListAsync();

            default:
                return await _context.Wallets.Include(t => t.IncomingTransactions).ToListAsync();
        }
    }

    public async Task<List<Transaction>> GetTransactionsAsync(Domain.Wallet wallet) 
    {
        var transactions = new List<Transaction>();
        
        if (!(wallet.IncomingTransactions == null))
        {
            GetTransactions(wallet.IncomingTransactions);

        }
   
        if (!(wallet.OutgoingTransactions == null))
        {
            GetTransactions(wallet.OutgoingTransactions);
        }

        return transactions;

    }

    public async Task NewTransfer(Domain.Wallet originWallet, Domain.Wallet destinationWallet, decimal amount, string description)
    {
        var walletOne = await _context.Wallets.FindAsync(originWallet.Id);
        var walletTwo = await _context.Wallets.FindAsync(destinationWallet.Id);

        if (!(walletOne == null || walletTwo == null)) 
        {
            if (walletOne.Balance >= amount)
            {
                walletOne.Balance -= amount;
                if (walletOne.OutgoingTransactions == null) 
                {
                    walletOne.OutgoingTransactions = new List<Transaction>();
                }

                var outgoingTransaction = new Transaction
                {
                    Amount = amount,
                    Date = DateTime.UtcNow,
                    Description = description,
                    WalletOutgoing = walletTwo
                };

                _context.Transactions.Add(outgoingTransaction);

                walletOne.OutgoingTransactions.Add(outgoingTransaction);

                walletTwo.Balance += amount;
                if (walletTwo.IncomingTransactions == null)
                {
                    walletTwo.IncomingTransactions = new List<Transaction>();

                }

                var incomingTransaction = new Transaction
                {
                    Amount = amount,
                    Date = DateTime.UtcNow,
                    Description = description,
                    WalletIncoming = walletOne
                };
                _context.Transactions.Add(incomingTransaction);

               walletTwo.IncomingTransactions.Add(incomingTransaction);


                _context.Update(walletOne);
                _context.Update(walletTwo);
                await _context.SaveChangesAsync();
            }
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
        var entity = await _context.Wallets.FindAsync(wallet.Id);
        entity.IncomingTransactions = wallet.IncomingTransactions;
        entity.OutgoingTransactions = wallet.OutgoingTransactions;
        var entries = _context.ChangeTracker.Entries(); foreach (var entry in entries) { Console.WriteLine($"{entry.Entity.GetType().Name} - {entry.State}"); }
        _context.Update(entity);
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

    public List<Transaction> GetTransactions(List<Transaction> transactions)
    {
        foreach (var transaction in transactions)
        {
            transactions.Add(transaction);
        }

        return (transactions);
    }
}
