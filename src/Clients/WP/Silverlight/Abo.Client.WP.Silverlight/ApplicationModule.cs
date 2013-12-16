using System.Diagnostics;
using System.Reflection;
using Abo.Client.Core.Infrastructure.Contracts;
using Abo.Client.Core.Infrastructure.Contracts.View;
using Abo.Client.Core.Infrastructure.Proxies;
using Abo.Client.Core.Managers;
using Abo.Client.Core.Model.Astral;
using Abo.Client.WP.Silverlight.Infrastructure;
using Abo.Client.WP.Silverlight.Navigation;
using Abo.Client.WP.Silverlight.Notifications.Toast;
using Abo.Client.WP.Silverlight.Seedwork;
using Abo.Client.WP.Silverlight.Seedwork.Navigation;
using Abo.Client.WP.Silverlight.ViewModels;
using Abo.Client.WP.Silverlight.ViewModels.OnlineHub;
using Abo.Client.WP.Silverlight.ViewModels.OnlineHub.Blocks;
using Abo.Client.WP.Silverlight.Views;
using Abo.Server.Application.DataTransferObjects.Utils;
using Abo.Server.Infrastructure.Protocol;
using Autofac;

namespace Abo.Client.WP.Silverlight
{
    public class ApplicationModule : LayerModule
    {
        private NavigationBuilder _navigationBuilder = null;

        protected override void OnMap(ContainerBuilder builder)
        {
            _navigationBuilder = new NavigationBuilder(builder);
            builder.RegisterInstance(_navigationBuilder).SingleInstance();

            builder.RegisterType<ProtobufSerializer>().As<IDtoSerializer>().SingleInstance();
            builder.RegisterType<DeviceInfo>().As<IDeviceInfo>().SingleInstance();
            builder.RegisterType<Storage>().As<IStorage>().SingleInstance();
            builder.RegisterType<NotificationService>().As<INotificationService>().SingleInstance();
            builder.RegisterType<SocketStreamTransportResource>().As<ITransportResource>().SingleInstance();
            builder.RegisterType<CommandBuffer>().AsSelf().SingleInstance();
            builder.RegisterType<CommandParser>().AsSelf().SingleInstance();
            builder.RegisterType<ConnectionManager>().AsSelf().SingleInstance();
            builder.RegisterType<CardRepository>().AsSelf().SingleInstance();
            builder.RegisterType<RequestsHandler>().AsSelf().SingleInstance();

            builder.RegisterAssemblyTypes(typeof(ServiceProxyBase).Assembly).Where(i => i.IsSubclassOf(typeof(ServiceProxyBase))).AsSelf().SingleInstance();
            builder.RegisterAssemblyTypes(typeof(ManagerBase).Assembly).Where(i => i.IsSubclassOf(typeof(ManagerBase))).AsSelf().SingleInstance();

            builder.RegisterType<AboNavigationManager>().As<NavigationManagerBase>().SingleInstance();
            builder.RegisterType<PopupController>().AsSelf().SingleInstance();
            builder.RegisterType<ToastNotificator>().AsSelf().SingleInstance();

            _navigationBuilder
                .RegisterViewModel<FrameViewModel>().StaticResource().WithoutView()
                .RegisterViewModel<LoginViewModel>().Singleton().ForView<LoginPage>()
                .RegisterViewModel<ChatOnlineHubViewModel>().WithoutView()
                .RegisterViewModel<BlacklistViewModel>().WithoutView()
                .RegisterViewModel<FriendsViewModel>().WithoutView()
                .RegisterViewModel<HallOfFameViewModel>().WithoutView()
                .RegisterViewModel<PlayersOnlineHubViewModel>().WithoutView()
                .RegisterViewModel<SearchPlayersViewModel>().WithoutView()
                .RegisterViewModel<ChatOnlineHubViewModel>().WithoutView()
                .RegisterViewModel<OnlineProfileViewModel>().WithoutView()
                .RegisterViewModel<RegistrationViewModel>().Singleton().ForView<RegistrationPage>()
                .RegisterViewModel<CardsCatalogViewModel>().ForView<CardsCatalogPage>()
                .RegisterViewModel<MainMenuViewModel>().Singleton().ForView<MainMenuPage>()
                .RegisterViewModel<SinglePlayerViewModel>().ForView<SinglePlayerOptionsPage>()
                .RegisterViewModel<OnlinePlayerDetailsViewModel>().Singleton().ForView<OnlinePlayerDetails>()
                .RegisterViewModel<PhotoSelectorViewModel>().Singleton().ForView<PhotoSelectionView>()
                .RegisterViewModel<AggregatedOnlineHubViewModel>().ForView<OnlineHubPage>()
                .RegisterViewModel<InputTextViewModel>().ForView<InputTextView>();
        }

        public override void OnPostContainerBuild(IContainer container)
        {
            _navigationBuilder.RegisterStaticViewModels(container.Resolve);
        }
    }
}
