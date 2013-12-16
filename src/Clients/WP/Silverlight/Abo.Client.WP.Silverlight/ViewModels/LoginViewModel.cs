using System;
using System.Windows;
using System.Windows.Input;
using Abo.Client.Core.Managers;
using Abo.Client.Core.Model.Enums;
using Abo.Client.WP.Silverlight.Resources;
using Abo.Client.WP.Silverlight.Seedwork;
using Abo.Client.WP.Silverlight.Seedwork.Controls;
using GalaSoft.MvvmLight.Command;

namespace Abo.Client.WP.Silverlight.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly AccountManager _accountManager;
        private string _name;
        private string _password;

        public LoginViewModel(AccountManager accountManager)
        {
            _accountManager = accountManager;
            NameValidator = new Validator(IsNameValid);
            PasswordValidator = new Validator(IsPasswordValid);
        }

        private string IsPasswordValid(string arg)
        {
            if (string.IsNullOrEmpty(arg))
                return AppResources.ShouldntBeEmpty;

            if (arg.Length < 3)
                return AppResources.TooSmall;

            if (arg.Length > 100)
                return AppResources.TooLarge;

            return string.Empty;
        }

        private string IsNameValid(string arg)
        {
            if (string.IsNullOrEmpty(arg))
                return AppResources.ShouldntBeEmpty;

            if (arg.Length < 2)
                return AppResources.TooSmall;

            if (arg.Length > 25)
                return AppResources.TooLarge;

            return string.Empty;
        }

        public Validator NameValidator { get; private set; }

        public Validator PasswordValidator { get; private set; }

        public string Name
        {
            get { return _name; }
            set { SetPropertyAndRefreshCanExecute(ref _name, value); }
        }

        public string Password
        {
            get { return _password; }
            set { SetPropertyAndRefreshCanExecute(ref _password, value); }
        }
    
        public ICommand SubmitCommand
        {
            get { return new RelayCommand(OnSubmit); }
        }

        private async void OnSubmit()
        {
            if (!(PasswordValidator.Validate() & NameValidator.Validate()))
                return;

            IsBusy = true;
            var result = await _accountManager.ValidateAccount(Name, Password);
            IsBusy = false;
            switch (result)
            {
                case AuthenticationResult.Success:
                    ServiceLocator.Resolve<MainMenuViewModel>().Show();
                    break;
                case AuthenticationResult.Banned:
                    MessageBox.Show(AppResources.DeviceIsBanned);
                    break;
                case AuthenticationResult.ReconnectFailed:
                case AuthenticationResult.ServerIsBusy:
                    MessageBox.Show(AppResources.ServerIsBusyAtTheMomentDesc);
                    break;
                case AuthenticationResult.InvalidNameOrPassword:
                    MessageBox.Show(AppResources.InvalidNameOrPassword);
                    break;
                case AuthenticationResult.OldClient:
                    MessageBox.Show(AppResources.VersionIsOutOfDate);
                    break;
            }
        }
    }
}