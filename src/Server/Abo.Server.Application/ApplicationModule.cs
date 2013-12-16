using System.Reflection;
using Abo.Server.Application.Agents;
using Abo.Server.Application.DataTransferObjects.Utils;
using Abo.Server.Application.Seedwork;
using Abo.Server.Application.Services.Helpers;
using Autofac;
using Module = Autofac.Module;

namespace Abo.Server.Application
{
    public class ApplicationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<RequestsHandler>().AsSelf().SingleInstance();
            builder.RegisterType<AgentManager>().AsSelf().SingleInstance();
            builder.RegisterType<ProfileChangesNotificator>().AsSelf().SingleInstance();

            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).Where(i => i.IsSubclassOf(typeof(AppService))).AsSelf().SingleInstance();
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).Where(i => i.IsSubclassOf(typeof(ScheduledAgentBase))).AsSelf().SingleInstance();

            base.Load(builder);
        }
    }
}
