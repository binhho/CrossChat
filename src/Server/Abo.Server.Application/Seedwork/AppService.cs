using Abo.Server.Domain.Seedwork;
using Abo.Utils.Logging;

namespace Abo.Server.Application.Seedwork
{
    public abstract class AppService
    {
        protected IUnitOfWorkFactory UnitOfWorkFactory { get; private set; }
        protected ITransactionFactory TransactionFactory { get; private set; }

        protected AppService(
            IUnitOfWorkFactory unitOfWorkFactory,
            ITransactionFactory transactionFactory)
        {
            UnitOfWorkFactory = unitOfWorkFactory;
            TransactionFactory = transactionFactory;
        }

        protected ILogger Logger
        {
            get { return LogFactory.GetLogger(GetType().Name); }
        }
    }
}
