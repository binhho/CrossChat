using System;
using Abo.Server.Application;
using Abo.Server.Application.Agents;
using Abo.Server.Application.Contracts;
using Abo.Server.Application.Seedwork;
using Abo.Server.Infrastructure;
using Autofac;

namespace Abo.Server.ConsoleHost
{
    public static class Bootstrapper
    {
        private static bool _isRunning = false;

        public static IContainer Run()
        {
            if (_isRunning)
                throw new InvalidOperationException();
            _isRunning = true;

            var builder = new ContainerBuilder();
            builder.RegisterModule<ApplicationModule>();
            builder.RegisterModule<InfrastructureModule>();

            var container = builder.Build();
            ServiceLocator.Init(container);
            container.Resolve<IInfrastructureInitializator>().Init();
            container.Resolve<AgentManager>().Run();

            return container;
        }
    }
}
