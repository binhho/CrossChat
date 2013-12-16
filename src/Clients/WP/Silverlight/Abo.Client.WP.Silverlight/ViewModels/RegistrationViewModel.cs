using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Abo.Client.Core.Managers;
using Abo.Client.Core.Model.Enums;
using Abo.Client.WP.Silverlight.Assets.Flags;
using Abo.Client.WP.Silverlight.Resources;
using Abo.Client.WP.Silverlight.Seedwork;
using Abo.Client.WP.Silverlight.Seedwork.Controls;
using GalaSoft.MvvmLight.Command;

namespace Abo.Client.WP.Silverlight.ViewModels
{
    public class RegistrationViewModel : BaseViewModel
    {     
        private readonly AccountManager _accountManager;
        private readonly AppManager _appManager;
        private string _name;
        private string _password;
        private int _selectedAge;
        private string[] _sexes;
        private string _selectedSex;
        private string _selectedCountry;
        private string[] _countries;
        private int[] _ages;

        private const int FemaleBuiltinPhotoId = -25;
        private const int MaleBuiltinPhotoId = -22;

        private readonly List<string> _namesInUse = new List<string>();

        public RegistrationViewModel(AccountManager accountManager, AppManager appManager)
        {
            _accountManager = accountManager;
            _appManager = appManager;

            NameValidator = new Validator(IsNameValid);
            PasswordValidator = new Validator(IsPasswordValid);

            Sexes = new[] { AppResources.Male, AppResources.Female };
            Countries = CountriesList.Countries; //TODO load from file
            Ages = Enumerable.Range(13, 120).ToArray();

            SelectedSex = Sexes[0];
            SelectedCountry = TryGetCurrentCountry();
            SelectedAge = 18;
        }

        private string TryGetCurrentCountry()
        {
            //TODO: move to core and improve
            var locale = CultureInfo.CurrentCulture.Name.ToLower();
            if (locale == "ru-ru" || locale == "ru")
                return "Russia";
            if (locale == "en-gb" || locale == "en")
                return "United Kingdom";

            return "United States";
        }

        public Validator NameValidator { get; private set; }

        public Validator PasswordValidator { get; private set; }

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

            if (_namesInUse.Contains(arg))
                return AppResources.IsAlreadyInUse;

            return string.Empty;
        }

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

        public int SelectedAge
        {
            get { return _selectedAge; }
            set { SetPropertyAndRefreshCanExecute(ref _selectedAge, value); }
        }

        public int[] Ages
        {
            get { return _ages; }
            set { SetPropertyAndRefreshCanExecute(ref _ages, value); }
        }

        public string[] Sexes
        {
            get { return _sexes; }
            set { SetProperty(ref _sexes, value); }
        }

        public string SelectedSex
        {
            get { return _selectedSex; }
            set { SetPropertyAndRefreshCanExecute(ref _selectedSex, value); }
        }

        public string[] Countries
        {
            get { return _countries; }
            set { SetProperty(ref _countries, value); }
        }

        public string SelectedCountry
        {
            get { return _selectedCountry; }
            set { SetPropertyAndRefreshCanExecute(ref _selectedCountry, value); }
        }

        public ICommand SubmitCommand
        {
            get { return new RelayCommand(OnSubmit); }
        }

        public ICommand LoginCommand
        {
            get { return new RelayCommand(() => ServiceLocator.Resolve<LoginViewModel>().Show()); }
        }

        private async void OnSubmit()
        {
            if (!(PasswordValidator.Validate() & NameValidator.Validate()))
                return;

            bool isMale = SelectedSex == Sexes[0];

            IsBusy = true;
            var registrationResult = await  _accountManager.Register(Name, Password, SelectedAge, isMale, SelectedCountry, isMale ? MaleBuiltinPhotoId : FemaleBuiltinPhotoId);
            IsBusy = false;

            switch (registrationResult)
            {
                case RegistrationResult.Success:
                    ServiceLocator.Resolve<MainMenuViewModel>().Show();
                    break;
                case RegistrationResult.NameIsInUse:
                    _namesInUse.Add(Name);
                    NameValidator.Validate();
                    break;
                case RegistrationResult.InvalidData:
                    MessageBox.Show(AppResources.DataIsInvalid);
                    break;
                case RegistrationResult.OldClient:
                    MessageBox.Show(AppResources.VersionIsOutOfDate);
                    break;
                case RegistrationResult.ServerIsBusy:
                    MessageBox.Show(AppResources.ServerIsBusyAtTheMomentDesc);
                    break;
                case RegistrationResult.Banned:
                    MessageBox.Show(AppResources.DeviceIsBanned);
                    break;
            }
        }

        private bool OnCanSubmit()
        {
            return !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Password) && SelectedAge > 13;
        }
    }
}