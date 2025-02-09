using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kata.Wallet.Dtos;

namespace Kata.Wallet.Services
{
    public interface ITransactionService
    {
        Task<List<TransactionDto>> GetTransactionsAsync(int walletId);
    }
}
