using System.Threading.Tasks;
using Abo.Client.Core.Managers;
using Abo.Client.Core.Model;
using Abo.Utils.Collections;

namespace Abo.Client.WP.Silverlight.ViewModels.OnlineHub.Blocks
{
    public class HallOfFameViewModel : OnlineHubViewModelBase
    {
        private readonly HallOfFameManager _hallOfFameManager;
        private IRoObservableCollection<PlayerViewModel> _top;

        public HallOfFameViewModel(HallOfFameManager hallOfFameManager)
        {
            _hallOfFameManager = hallOfFameManager;
        }

        protected override async Task OnReload()
        {
            await _hallOfFameManager.ReloadTopAsync();
            Top = _hallOfFameManager.Top.Select(CreateFriend);
        }

        private HallOfFamePlayer CreateFriend(PlayerPositionPair playerPositionPair)
        {
            return new HallOfFamePlayer(playerPositionPair.Position, playerPositionPair.Player);
        }

        public IRoObservableCollection<PlayerViewModel> Top
        {
            get { return _top; }
            private set { SetProperty(ref _top, value); }
        }
    }
}