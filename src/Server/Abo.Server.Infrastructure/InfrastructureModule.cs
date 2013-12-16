using Abo.Server.Application;
using Abo.Server.Application.Contracts;
using Abo.Server.Application.Sessions;
using Abo.Server.Domain.Seedwork;
using Abo.Server.Infrastructure.FileSystem;
using Abo.Server.Infrastructure.Logging;
using Abo.Server.Infrastructure.Persistence.EF;
using Abo.Server.Infrastructure.Persistence.EF.Transaction;
using Abo.Server.Infrastructure.Protocol;
using Abo.Server.Infrastructure.Push;
using Abo.Server.Infrastructure.Serialization;
using Abo.Server.Infrastructure.Serialization.Implementations;
using Abo.Server.Infrastructure.Transport;
using Abo.Utils.Logging;
using Autofac;

namespace Abo.Server.Infrastructure
{
    public class InfrastructureModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            LogFactory.Initialize(typeName => new ConsoleLogger(typeName));

            builder.RegisterType<InfrastructureInitializator>().As<IInfrastructureInitializator>().SingleInstance();
            builder.RegisterType<UnitOfWorkFactory>().As<IUnitOfWorkFactory>().SingleInstance();
            builder.RegisterType<TransactionFactory>().As<ITransactionFactory>().SingleInstance();
            builder.RegisterType<AboSocketServer>().AsSelf().As<ISessionManager>().SingleInstance();
            builder.RegisterType<HardcodedSettings>().As<ISettings>().SingleInstance();
            builder.RegisterType<FileStorage>().As<IFileStorage>().SingleInstance();
            builder.RegisterType<ProtobufDtoSerializer>().As<IDtoSerializer>().SingleInstance();
            builder.RegisterType<CommandParser>().AsSelf().SingleInstance();
            builder.RegisterType<TransportPerformanceMeasurer>().AsSelf().SingleInstance();
            builder.RegisterType<WindowsPhonePushSender>().As<IPushNotificator>().SingleInstance();
            
            base.Load(builder);
        }
    }
}
