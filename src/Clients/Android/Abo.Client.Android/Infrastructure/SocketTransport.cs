using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using Abo.Client.Core.Infrastructure.Contracts;

namespace Abo.Client.Android.Infrastructure
{
    public class SocketTransport : ITransportResource
    {
        private TcpClient _tcpClient = null;
        private NetworkStream _networkStream;

        public async Task ConnectAsync()
        {
            IsConnecting = true;
            _tcpClient = new TcpClient();
            await _tcpClient.ConnectAsync(GlobalConfig.IpAddress, GlobalConfig.Port);
            _networkStream = _tcpClient.GetStream();
            IsConnecting = false;
            Task.Run(() => StartListening());
        }

        private void StartListening()
        {
            while (true)
            {
                byte[] bytes = new byte[_tcpClient.ReceiveBufferSize];
                _networkStream.Read(bytes, 0, (int)_tcpClient.ReceiveBufferSize);
                DataReceived(bytes);
            }
        }

        public Task DisconnectAsync()
        {
            _tcpClient.Close();
            return Task.FromResult<object>(null);
        }

        public event EventHandler ConnectionError = delegate { };
        
        public event Action<byte[]> DataReceived = delegate { };

        public void SendData(byte[] data)
        {
            _networkStream.Write(data, 0, data.Length);
        }

        public bool IsConnecting { get; private set; }
        public bool IsConneced { get; private set; }
    }
}