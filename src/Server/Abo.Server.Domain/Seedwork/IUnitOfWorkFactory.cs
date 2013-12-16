namespace Abo.Server.Domain.Seedwork
{
    public interface IUnitOfWorkFactory
    {
        IUnitOfWork Create();
    }
}