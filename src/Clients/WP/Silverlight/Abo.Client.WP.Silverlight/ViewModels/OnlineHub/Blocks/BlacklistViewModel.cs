using System.Threading.Tasks;
using Abo.Client.Core.Managers;
using Abo.Client.Core.Model;
using Abo.Utils.Collections;

namespace Abo.Client.WP.Silverlight.ViewModels.OnlineHub.Blocks
{
    public class BlacklistViewModel : OnlineHubViewModelBase
    {
        private IRoObservableCollection<PlayerViewModel> _blacklist;
        private readonly BlacklistManager _blacklistManager;

        public BlacklistViewModel(BlacklistManager blacklistManager)
        {
            _blacklistManager = blacklistManager;
        }

        protected override async Task OnReload()
        {
            await _blacklistManager.ReloadBlacklistAsync();
            Blacklist = _blacklistManager.Blacklist.Select(CreateFriend);
        }
        
        private PlayerViewModel CreateFriend(User user)
        {
            return new PlayerViewModel(user);
        }

        public IRoObservableCollection<PlayerViewModel> Blacklist
        {
            get { return _blacklist; }
            private set { SetProperty(ref _blacklist, value); }
        }
    }
}