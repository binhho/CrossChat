using Autofac;

namespace Abo.Client.Console
{
    public static class Bootstrapper
    {
        private static bool _isRun = false;
        private static IContainer _container;

        public static IContainer Run()
        {
            if (_isRun)
                return _container;
            _isRun = true;

            var builder = new ContainerBuilder();
            builder.RegisterModule<ApplicationModule>();
            _container = builder.Build();

            return _container;
        }
    }
}
