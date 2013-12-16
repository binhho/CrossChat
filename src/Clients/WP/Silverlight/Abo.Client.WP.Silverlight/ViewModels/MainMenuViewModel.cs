using System;
using System.Windows;
using System.Windows.Input;
using Abo.Client.Core.Managers;
using Abo.Client.Core.Model.Enums;
using Abo.Client.WP.Silverlight.Notifications.Toast;
using Abo.Client.WP.Silverlight.Resources;
using Abo.Client.WP.Silverlight.Seedwork;
using Abo.Client.WP.Silverlight.ViewModels.OnlineHub;
using GalaSoft.MvvmLight.Command;

namespace Abo.Client.WP.Silverlight.ViewModels
{
    public class MainMenuViewModel : BaseViewModel
    {
        private readonly AppManager _appManager;
        private readonly SinglePlayerViewModel _singlePlayerViewModel;
        private readonly CardsCatalogViewModel _cardsCatalog;

        public MainMenuViewModel(AppManager appManager, 
            SinglePlayerViewModel singlePlayerViewModel,
            CardsCatalogViewModel cardsCatalog)
        {
            _appManager = appManager;
            _singlePlayerViewModel = singlePlayerViewModel;
            _cardsCatalog = cardsCatalog;
        }

        public ICommand ShowOnlineHubCommand
        {
            get { return new RelayCommand(ShowOnlineHub); }
        }

        public ICommand ShowCardsCatalogCommand
        {
            get { return new RelayCommand(ShowCardsCatalog); }
        }

        public ICommand ShowSinglePlayerCommand
        {
            get { return new RelayCommand(ShowSinglePlayer); }
        }

        public override void OnNavigatedTo()
        {
            ServiceLocator.Resolve<ToastNotificator>().DisplayJustText("this is toast notification!");
            _appManager.EnsureConnectionIsClosed();
            base.OnNavigatedTo();
        }

        private void ShowSinglePlayer()
        {
            _singlePlayerViewModel.Show();
        }

        private void ShowCardsCatalog()
        {
            _cardsCatalog.Show();
        }

        private async void ShowOnlineHub()
        {
            IsBusy = true;
            var authResult = await _appManager.InitAsync();
            IsBusy = false;
            switch (authResult)
            {
                case AuthenticationResult.Success:
                    ServiceLocator.Resolve<AggregatedOnlineHubViewModel>().Show();
                    break;
                case AuthenticationResult.InvalidNameOrPassword:
                    var goToRegistration = MessageBox.Show(AppResources.AuthenticationFailedAccountDoesntExists, AppResources.AuthenticationFailed, MessageBoxButton.OKCancel) == MessageBoxResult.OK;
                    if (goToRegistration)
                        ServiceLocator.Resolve<RegistrationViewModel>().Show();
                    break;
                case AuthenticationResult.Banned:
                    MessageBox.Show(AppResources.DeviceIsBanned);
                    break;
                case AuthenticationResult.ReconnectFailed:
                case AuthenticationResult.ServerIsBusy:
                    MessageBox.Show(AppResources.ServerIsBusyAtTheMomentDesc);
                    break;
                case AuthenticationResult.OldClient:
                    MessageBox.Show(AppResources.VersionIsOutOfDate);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}