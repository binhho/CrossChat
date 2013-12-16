using System.Threading.Tasks;
using System.Windows.Input;
using Abo.Client.WP.Silverlight.Infrastructure;
using Abo.Client.WP.Silverlight.Notifications.Toast;
using Abo.Client.WP.Silverlight.Seedwork;
using GalaSoft.MvvmLight.Command;

namespace Abo.Client.WP.Silverlight.ViewModels.OnlineHub
{
    public class OnlineHubViewModelBase : BaseViewModel
    {
        private bool _isInited;
        private bool _isLoading;

        public ICommand ReloadCommand
        {
            get { return new RelayCommand(Reload); }
        }

        protected virtual async Task OnReload() {}

        private async void Reload()
        {
            if (_isInited)
                return;
            _isInited = true;
            IsLoading = true;
            await Task.Delay(800);
            await OnReload();
            UIDispatcher.Current.Dispatch(() => IsLoading = false);
        }

        public bool IsLoading
        {
            get { return _isLoading; }
            set { SetProperty(ref _isLoading, value); }
        }
    }
}
