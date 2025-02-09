using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kata.Wallet.Domain
{
    public class DifferentCurrencyException : WalletTransactionException
    {
        public DifferentCurrencyException() { }

        public DifferentCurrencyException(string message)
            : base(message) { }

        public DifferentCurrencyException(string message, Exception inner)
            : base(message, inner) { }

        public DifferentCurrencyException(int errorCode, string entity, string message)
            : base(message, errorCode, entity) { }
    }
}
