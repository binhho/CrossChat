using System;
using System.Threading.Tasks;

namespace Abo.Client.Core.Infrastructure.Contracts
{
    public interface ITransportResource
    {
        Task ConnectAsync();

        Task DisconnectAsync();

        event EventHandler ConnectionError;

        event Action<byte[]> DataReceived;

        void SendData(byte[] data);

        bool IsConnecting { get; }

        bool IsConneced { get; }
    }
}
