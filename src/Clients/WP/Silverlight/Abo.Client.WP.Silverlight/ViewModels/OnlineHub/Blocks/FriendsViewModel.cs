using System.Threading.Tasks;
using Abo.Client.Core.Managers;
using Abo.Client.Core.Model;
using Abo.Utils.Collections;

namespace Abo.Client.WP.Silverlight.ViewModels.OnlineHub.Blocks
{
    public class FriendsViewModel : OnlineHubViewModelBase
    {
        private readonly FriendsManager _friendsManager;
        private IRoObservableCollection<PlayerViewModel> _friends;

        public FriendsViewModel(FriendsManager friendsManager)
        {
            _friendsManager = friendsManager;
        }

        protected override async Task OnReload()
        {
            await _friendsManager.ReloadFriendsAsync();
            Friends = _friendsManager.Friends.Select(CreateFriend);
        }

        private PlayerViewModel CreateFriend(User user)
        {
            return new PlayerViewModel(user);
        }

        public IRoObservableCollection<PlayerViewModel> Friends
        {
            get { return _friends; }
            private set { SetProperty(ref _friends, value); }
        }
    }
}
