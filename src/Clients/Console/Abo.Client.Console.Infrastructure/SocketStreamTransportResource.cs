using System;
using System.Threading.Tasks;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using Abo.Client.Core.Infrastructure.Contracts;
using Abo.Client.Core.Infrastructure.Contracts.EventArguments;
using Abo.Server.Infrastructure.Protocol;

namespace Abo.Client.Desktop.Infrastructure
{
    public class SocketStreamTransportResource : ITransportResource
    {
        private StreamSocket _socket;
        private DataWriter _dataWriter;
        private DataReader _dataReader;
        
        public async Task ConnectAsync()
        {
            _socket = new StreamSocket();
            await _socket.ConnectAsync(new HostName(GlobalConfig.IpAddress), GlobalConfig.Port);
            _dataWriter = new DataWriter(_socket.OutputStream);
            _dataReader = new DataReader(_socket.InputStream) { InputStreamOptions = InputStreamOptions.Partial };
            StartListening();
        }

        private async void StartListening()
        {
            try
            {
                while (true)
                {
                    uint readLength = await _dataReader.LoadAsync(1024);
                    System.Console.WriteLine("Got {0}({1}) bytes", readLength, _dataReader.UnconsumedBufferLength);
                    byte[] buffer = new byte[_dataReader.UnconsumedBufferLength];
                    _dataReader.ReadBytes(buffer);
                    DataReceived(buffer);
                }
            }
            catch (Exception)
            {
            }
        }

        public async Task DisconnectAsync()
        {
            _socket.Dispose();
            _socket = null;
        }

        public event EventHandler ConnectionError = delegate { };

        public event Action<byte[]> DataReceived = delegate { };

        public bool IsConnecting { get; private set; }

        public bool IsConneced { get; private set; }

        public async void SendData(byte[] bytes)
        {
            _dataWriter.WriteBytes(bytes);
            uint result = await _dataWriter.StoreAsync();
            await _dataWriter.FlushAsync();
            System.Console.WriteLine("Sent {0} ({1}) bytes", bytes.Length, result);
            //throw new NotImplementedException();
        }
    }
}
