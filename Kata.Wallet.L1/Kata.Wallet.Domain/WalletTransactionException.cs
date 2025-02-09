using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kata.Wallet.Domain
{
    public class WalletTransactionException : Exception
    {
        public int ErrorCode { get; set; }

        // Entidad afectada
        public string? Entity { get; set; }

        // Constructor sin parametros
        public WalletTransactionException() { }

        // Constructor con mensaje de error
        public WalletTransactionException(string message)
            : base(message) { }

        // Constructor con mensaje de error y excepcion interna
        public WalletTransactionException(string message, Exception inner)
            : base(message, inner) { }

        // Constructor con mensaje de error, codigo de error y entidad
        public WalletTransactionException(string message, int errorCode, string entity)
            : base(message)
        {
            ErrorCode = errorCode;
            Entity = entity;
        }
    }
}
