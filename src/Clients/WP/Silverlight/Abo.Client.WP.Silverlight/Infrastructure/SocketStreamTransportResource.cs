using System;
using System.Threading.Tasks;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using Abo.Client.Core.Infrastructure.Contracts;
using Abo.Client.Core.Infrastructure.Contracts.EventArguments;
using Abo.Server.Infrastructure.Protocol;

namespace Abo.Client.WP.Silverlight.Infrastructure
{
    public class SocketStreamTransportResource : ITransportResource
    {
        private readonly CommandBuffer _commandBuffer;
        private readonly CommandParser _commandParser;
        private StreamSocket _socket;
        private DataWriter _dataWriter;
        private DataReader _dataReader;

        public SocketStreamTransportResource(CommandBuffer commandBuffer, CommandParser commandParser)
        {
            _commandBuffer = commandBuffer;
            _commandParser = commandParser;
            _commandBuffer.CommandAssembled += _commandBuffer_OnCommandAssembled;
        }

        private void _commandBuffer_OnCommandAssembled(Command cmd)
        {
            CommandReceived(this, new CommandEventArgs(cmd));
        }

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
                    byte[] buffer = new byte[_dataReader.UnconsumedBufferLength];
                    _dataReader.ReadBytes(buffer);
                    _commandBuffer.AppendBytes(buffer);
                }
            }
            catch (Exception)
            {
            }
        }

        public async Task DisconnectAsync()
        {
            if (_socket == null)
                return;
            _socket.Dispose();
            _socket = null;
            await Task.Delay(1500);
        }

        public event EventHandler ConnectionError = delegate { };

        public event EventHandler<CommandEventArgs> CommandReceived = delegate { };

        public bool IsConnecting { get; private set; }

        public bool IsConneced { get; private set; }

        public async void SendData(Command command)
        {
            try
            {
                var bytes = _commandParser.ToBytes(command);
                _dataWriter.WriteBytes(bytes);
                uint result = await _dataWriter.StoreAsync();
                await _dataWriter.FlushAsync();
            }
            catch (Exception)
            {
            }
        }
    }
}
