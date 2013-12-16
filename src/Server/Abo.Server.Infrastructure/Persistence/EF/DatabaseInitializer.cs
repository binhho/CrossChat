using System.Data.Entity;

namespace Abo.Server.Infrastructure.Persistence.EF
{
    public class DatabaseInitializer : CreateDatabaseIfNotExists<UnitOfWork>
    {
        protected override void Seed(UnitOfWork context)
        {
            base.Seed(context);
        }
    }
}
