using System.Transactions;
using Abo.Server.Domain.Seedwork;

namespace Abo.Server.Infrastructure.Persistence.EF.Transaction
{
    public class Transaction : ITransaction
    {
        private readonly TransactionScope _innerTransaction;

        public Transaction()
        {
            _innerTransaction = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted });
        }
        public void Dispose()
        {
            _innerTransaction.Dispose();
        }

        public void Complete()
        {
            _innerTransaction.Complete();
        }
    }
}
