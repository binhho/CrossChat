using System;

namespace Abo.Server.Domain.Seedwork
{
    public interface ITransaction : IDisposable
    {
        void Complete();
    }
}