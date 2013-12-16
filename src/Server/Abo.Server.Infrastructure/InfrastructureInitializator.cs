using System;
using System.Data.Entity;
using System.Threading.Tasks;
using Abo.Server.Application;
using Abo.Server.Application.Contracts;
using Abo.Server.Infrastructure.Persistence.EF;
using Abo.Server.Infrastructure.Transport;
using Abo.Utils.Logging;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Config;
using SuperSocket.SocketBase.Logging;

namespace Abo.Server.Infrastructure
{
    public class InfrastructureInitializator : IInfrastructureInitializator
    {
        private readonly ISettings _settings;
        private readonly AboSocketServer _aboSocketServer;
        private static readonly ILogger Logger = LogFactory.GetLogger<InfrastructureInitializator>();

        public InfrastructureInitializator(ISettings settings, AboSocketServer aboSocketServer)
        {
            _settings = settings;
            _aboSocketServer = aboSocketServer;
        }
        
        public async void Init()
        {
            Logger.Info("Initing...");
            var config = new ServerConfig
                {
                    Port = _settings.ServerPort,
                    Ip = "Any", 
                    MaxConnectionNumber = 2000,
                    Mode = SocketMode.Tcp,
                    //ReceiveBufferSize = 1024,
                    //SendBufferSize = 1024,
                    Name = "AboSocketServer",
                    DisableSessionSnapshot = true,
                    LogAllSocketException = false,
                    LogBasicSessionActivity = false,
                    LogCommand = false, 
                    //LogFactory = "DefaultLogFactory"
                };

            var setuped = _aboSocketServer.Setup(config);
            var started = _aboSocketServer.Start();
            Database.SetInitializer(new DropCreateDatabaseAlways<UnitOfWork>());
            //Database.SetInitializer(new DropCreateDatabaseIfModelChanges<UnitOfWork>());
            Logger.Info("Init completed {0}({1})", setuped, started);
        }
    }
}
