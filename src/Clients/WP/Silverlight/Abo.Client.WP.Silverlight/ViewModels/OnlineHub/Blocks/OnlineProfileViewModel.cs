using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Abo.Client.Core.Managers;
using Abo.Client.WP.Silverlight.Seedwork;
using GalaSoft.MvvmLight.Command;

namespace Abo.Client.WP.Silverlight.ViewModels.OnlineHub.Blocks
{
    public class OnlineProfileViewModel : OnlineHubViewModelBase
    {
        private readonly PhotoSelectorViewModel _photoSelectorViewModel;
        private readonly AccountManager _accountManager;
        private PlayerViewModel _player;

        public OnlineProfileViewModel(PhotoSelectorViewModel photoSelectorViewModel, AccountManager accountManager)
        {
            _photoSelectorViewModel = photoSelectorViewModel;
            _accountManager = accountManager;
            Player = new PlayerViewModel(accountManager.CurrentUser);
        }
        
        public PlayerViewModel Player
        {
            get { return _player; }
            set { SetProperty(ref _player, value); }
        }

        public ICommand ChangePhotoCommand
        {
            get { return new RelayCommand(() => _photoSelectorViewModel.SelectPhoto(false)); }
        }

        public ICommand DeactivateCommand
        {
            get { return new RelayCommand(Deactivate); }
        }

        private async void Deactivate()
        {
            if (MessageBox.Show("Deactivation", "Are you sure you want to deactivate your account? You won't be able to recover it.", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                await _accountManager.Deactivate();
                ServiceLocator.Resolve<RegistrationViewModel>().Show();
            }
        }
    }
}
