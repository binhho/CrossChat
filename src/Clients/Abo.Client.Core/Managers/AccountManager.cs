using System.Threading.Tasks;
using Abo.Client.Core.Infrastructure.Contracts;
using Abo.Client.Core.Infrastructure.Proxies;
using Abo.Client.Core.Model;
using Abo.Client.Core.Model.Enums;
using Abo.Server.Application.DataTransferObjects.Enums;
using Abo.Server.Application.DataTransferObjects.Requests;

namespace Abo.Client.Core.Managers
{
    public class AccountManager : ManagerBase
    {
        private readonly IDeviceInfo _deviceInfo;
        private readonly ProfileServiceProxy _profileServiceProxy;
        private readonly AuthenticationServiceProxy _authenticationServiceProxy;
        private readonly RegistrationServiceProxy _registrationServiceProxy;
        private readonly IStorage _storage;
        private User _currentUser = null;

        public AccountManager(ConnectionManager connectionManager, 
            IDeviceInfo deviceInfo,
            ProfileServiceProxy profileServiceProxy,
            AuthenticationServiceProxy authenticationServiceProxy,
            RegistrationServiceProxy registrationServiceProxy, 
            IStorage storage) : base(connectionManager)
        {
            _deviceInfo = deviceInfo;
            _profileServiceProxy = profileServiceProxy;
            _authenticationServiceProxy = authenticationServiceProxy;
            _registrationServiceProxy = registrationServiceProxy;
            _storage = storage;
        }

        public User CurrentUser
        {
            get { return _currentUser ?? (_currentUser = _storage.Get<User>()); }
            private set
            {
                _currentUser = value;
                _storage.Set(value);
                if (value != null)
                {
                    value.PropertyChanged += (s, e) => _storage.Set(CurrentUser);
                }
            }
        }

        public bool IsRegistered
        {
            get { return _storage.Get<bool>(); }
            private set { _storage.Set(value); }
        }

        public string AccountName
        {
            get { return _storage.Get<string>(); }
            private set { _storage.Set(value); }
        }

        public string AccountPassword
        {
            get { return _storage.Get<string>(); }
            private set { _storage.Set(value); }
        }

        private async Task<AuthenticationResult> ValidateAccount(string name, string password, bool keepConnectionOpen)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(password))
                return AuthenticationResult.InvalidNameOrPassword;

            await _deviceInfo.InitAsync();
            await ConnectionManager.ConnectAsync();
            var authResult = await _authenticationServiceProxy.Authenticate(
                new AuthenticationRequest { Huid = _deviceInfo.Huid, Name = name, Password = password });

            if (authResult.Result == AuthenticationResponseType.Success)
            {
                AccountName = name;
                AccountPassword = password;
                CurrentUser = ToEntity<User>(authResult.User);
                IsRegistered = true;
            }

            if (!keepConnectionOpen)
                await ConnectionManager.DisconnectAsync();

            return (AuthenticationResult)authResult.Result;
        }

        public async Task<AuthenticationResult> ValidateAccount()
        {
            if (!IsRegistered)
                return AuthenticationResult.InvalidNameOrPassword;
            return await ValidateAccount(AccountName, AccountPassword, true);
        }

        public async Task<AuthenticationResult> ValidateAccount(string name, string password)
        {
            if (!IsRegistered)
                return AuthenticationResult.InvalidNameOrPassword;
            return await ValidateAccount(AccountName, AccountPassword, false);
        }

        public async Task<RegistrationResult> Register(string name, string password, int age, bool sex, string country, int photoId)
        {
            await _deviceInfo.InitAsync();
            await ConnectionManager.ConnectAsync();
            var result = await _registrationServiceProxy.RegisterNewPlayer(
                new RegistrationRequest
                {
                    Age = age,
                    Name = name,
                    Password = password,
                    Sex = sex,
                    PhotoId = photoId,
                    Country = country,
                    Huid = _deviceInfo.Huid,
                    PushUri = _deviceInfo.PushUri,
                });
            
            if (result.Result == RegistrationResponseType.Success)
            {
                IsRegistered = true;
                CurrentUser = ToEntity<User>(result.User);
                AccountName = name;
                AccountPassword = password;
                await ConnectionManager.DisconnectAsync();
            }

            return (RegistrationResult)result.Result;
        }

        public async Task<bool> ChangePhotoToBuiltin(int builtinPhoto)
        {
            var response = await _profileServiceProxy.ChangePhoto(new ChangePhotoRequest { BuiltinPhotoId = builtinPhoto });
            if (response.Success)
            {
                CurrentUser.PhotoId = builtinPhoto;       
            }
            return response.Success;
        }

        public async Task<bool> ChangePhoto(byte[] photoData)
        {
            var response = await _profileServiceProxy.ChangePhoto(new ChangePhotoRequest { PhotoData = photoData });
            if (response.Success)
            {
                CurrentUser.PhotoId = response.PhotoId;
            }
            return response.Success;
        }

        public async Task Deactivate()
        {
            var response = await _registrationServiceProxy.Deactivate(new DeactivationRequest());
            IsRegistered = false;
            AccountName = "";
            AccountPassword = "";
            CurrentUser = null;
        }
    }
}
