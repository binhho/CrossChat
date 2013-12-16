using System.Windows.Input;
using Abo.Client.Core.Infrastructure.Contracts;
using Abo.Client.Core.Infrastructure.Proxies;
using Abo.Server.Application.DataTransferObjects.Enums;
using Abo.Server.Application.DataTransferObjects.Requests;
using Cirrious.MvvmCross.ViewModels;

namespace Abo.Client.Core.ViewModels
{
    public class LoginViewModel : MvxViewModel
    {
        private readonly AuthenticationServiceProxy _authenticationService;
        private readonly IDeviceInfo _deviceInfo;

        public LoginViewModel(AuthenticationServiceProxy authenticationService)
        {
            _authenticationService = authenticationService;
            _deviceInfo = null;
        }

        private string _name = "";
        public string Name
        {
            get { return _name; }
            set { _name = value; RaisePropertyChanged(() => Name); }
        }

        private string _password = "";
        public string Password
        {
            get { return _password; }
            set { _password = value; RaisePropertyChanged(() => Password); }
        }

        public ICommand LoginCommand
        {
            get { return new MvxCommand(Login); }
        }

        private async void Login()
        {
            var result = await _authenticationService.Authenticate(
                new AuthenticationRequest
                    {
                        Huid = _deviceInfo.Huid, 
                        Name = Name, 
                        Password = Password
                    });

            if (result.Result == AuthenticationResponseType.Success)
            {
                ShowViewModel<ChatViewModel>();
            }
        }
    }
}
