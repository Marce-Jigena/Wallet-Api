using AutoMapper;
using Kata.Wallet.Database;
using Kata.Wallet.Dtos;

namespace Kata.Wallet.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        public readonly IMapper _mapper;

        public TransactionService(
            IMapper mapper,
            ITransactionRepository transactionRepository)
        {
            _mapper = mapper;
            _transactionRepository = transactionRepository;
        }

        public async Task<List<TransactionDto>> GetTransactionsAsync(int walletId)
        {
            var transaction = await _transactionRepository.GetAllAsync(walletId);
            if (transaction == null)
            {
                throw new KeyNotFoundException("No se encontraron transacciones.");
            }
            var transactionDto = _mapper.Map<List<TransactionDto>>(transaction);

            return transactionDto;
        }
    }
}
