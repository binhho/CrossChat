using Abo.Server.Domain.Seedwork;

namespace Abo.Server.Infrastructure.Persistence.EF.Transaction
{
    public class TransactionFactory : ITransactionFactory
    {
        public ITransaction Create()
        {
            return new Transaction();
        }
    }
}