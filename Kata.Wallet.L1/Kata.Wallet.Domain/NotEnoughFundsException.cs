using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kata.Wallet.Domain
{
    public class NotEnoughFundsException : WalletTransactionException
    {
        public NotEnoughFundsException() { }

        public NotEnoughFundsException(string message)
            : base(message) { }

        public NotEnoughFundsException(string message, Exception inner)
            : base(message, inner) { }

        public NotEnoughFundsException(int errorCode, string entity, string message)
            : base(message, errorCode, entity) { }
    }
}
