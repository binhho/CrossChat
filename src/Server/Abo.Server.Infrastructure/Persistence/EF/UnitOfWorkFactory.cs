using Abo.Server.Application;
using Abo.Server.Application.Contracts;
using Abo.Server.Domain.Seedwork;

namespace Abo.Server.Infrastructure.Persistence.EF
{
    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly ISettings _settings;

        public UnitOfWorkFactory(ISettings settings)
        {
            _settings = settings;
        }

        public IUnitOfWork Create()
        {
            return new UnitOfWork();
        }
    }
}