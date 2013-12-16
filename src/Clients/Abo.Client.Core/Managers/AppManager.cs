using System.Threading.Tasks;
using Abo.Client.Core.Infrastructure.Contracts;
using Abo.Client.Core.Infrastructure.Proxies;
using Abo.Client.Core.Model.Enums;

namespace Abo.Client.Core.Managers
{
    public class AppManager : ManagerBase
    {
        private readonly IDeviceInfo _deviceInfo;
        private readonly AccountManager _accountManager;

        public AppManager(ConnectionManager connectionManager, 
            IDeviceInfo deviceInfo,
            AccountManager accountManager) : base(connectionManager)
        {
            _deviceInfo = deviceInfo;
            _accountManager = accountManager;
        }

        public async Task<AuthenticationResult> InitAsync()
        {
            await ConnectionManager.ConnectAsync();
            await _deviceInfo.InitAsync();
            var authResult = await _accountManager.ValidateAccount();
            return authResult;
        }

        public async void EnsureConnectionIsClosed()
        {
            await ConnectionManager.DisconnectAsync();
        }
    }
}
