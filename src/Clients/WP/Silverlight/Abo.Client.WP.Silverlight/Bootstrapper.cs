using Abo.Client.WP.Silverlight.Seedwork;
using Autofac;

namespace Abo.Client.WP.Silverlight
{
    public static class Bootstrapper
    {
        private static bool _isRan = false;

        public static void Run()
        {
            if (_isRan)
                return;
            _isRan = true;

            var builder = new ContainerBuilder();

            var modules = new LayerModule[] {
                    new ApplicationModule(),
                };

            foreach (var module in modules)
                builder.RegisterModule(module);

            var container = builder.Build();
            ServiceLocator.Init(container);

            foreach (var module in modules)
                module.OnPostContainerBuild(container);
        }
    }
}
