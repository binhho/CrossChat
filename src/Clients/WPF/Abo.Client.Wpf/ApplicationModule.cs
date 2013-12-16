using Abo.Client.Core.Infrastructure.Contracts;
using Abo.Client.Core.Infrastructure.Proxies;
using Abo.Client.Core.Managers;
using Abo.Client.Core.Model.Astral;
using Abo.Client.Desktop.Infrastructure;
using Abo.Server.Application.DataTransferObjects.Utils;
using Abo.Server.Infrastructure.Protocol;
using Abo.Utils;
using Autofac;

namespace Abo.Client.Wpf
{
    public class ApplicationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ProtobufSerializer>().As<IDtoSerializer>().SingleInstance();
            builder.RegisterType<DeviceInfo>().As<IDeviceInfo>().SingleInstance();
            builder.RegisterType<Storage>().As<IStorage>().SingleInstance();
            builder.RegisterType<SocketStreamTransportResource>().As<ITransportResource>().SingleInstance();
            builder.RegisterType<CommandBuffer>().AsSelf().SingleInstance();
            builder.RegisterType<CommandParser>().AsSelf().SingleInstance();
            builder.RegisterType<ConnectionManager>().AsSelf().SingleInstance();
            builder.RegisterType<CardRepository>().AsSelf().SingleInstance();
            builder.RegisterType<RequestsHandler>().AsSelf().SingleInstance();

            builder.RegisterAssemblyTypes(typeof(ServiceProxyBase).Assembly).Where(i => i.IsSubclassOf(typeof(ServiceProxyBase))).AsSelf().SingleInstance();
            builder.RegisterAssemblyTypes(typeof(ManagerBase).Assembly).Where(i => i.IsSubclassOf(typeof(ManagerBase))).AsSelf().SingleInstance();
            

            base.Load(builder);
        }
    }
}
