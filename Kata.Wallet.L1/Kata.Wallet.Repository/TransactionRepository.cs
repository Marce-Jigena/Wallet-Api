using Kata.Wallet.Domain;
using Microsoft.EntityFrameworkCore;

namespace Kata.Wallet.Database;

public class TransactionRepository : ITransactionRepository
{
    private readonly DataContext _context;

    public TransactionRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<List<Transaction>> GetAllAsync(int walletId)
    {
        var wallet = await _context.Wallets.Where(w => w.Id == walletId).Include(t => t.IncomingTransactions).Include(t => t.OutgoingTransactions).FirstAsync();
        var transactions = GetTransactions(wallet);

        return transactions;

    }

    public async Task<Transaction> GetByIdAsync(int id)
    {
        return await _context.Transactions.FindAsync(id);
    }

    public async Task AddAsync(Transaction transaction)
    {
        await _context.Transactions.AddAsync(transaction);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Transaction transaction)
    {
        _context.Transactions.Update(transaction);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var transaction = await _context.Transactions.FindAsync(id);
        if (transaction != null)
        {
            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();
        }
    }

    public List<Transaction> GetTransactions(Domain.Wallet wallet)
    {
        List<Transaction> transactions = new List<Transaction>();
        if (!(wallet.IncomingTransactions == null))
        {
            foreach (var transaction in wallet.IncomingTransactions)
            {
                transactions.Add(transaction);
            }
        }

        if (!(wallet.OutgoingTransactions == null))
        {
            foreach (var transaction in wallet.OutgoingTransactions)
            {
                transactions.Add(transaction);
            }
        }

        return (transactions);
    }
}
