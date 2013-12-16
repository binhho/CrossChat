using System.Threading.Tasks;

namespace Abo.Client.Core.Infrastructure.Contracts
{
    public interface IDeviceInfo
    {
        Task InitAsync();

        string Huid { get; }

        string PushUri { get; }
    }
}
