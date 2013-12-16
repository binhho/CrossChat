using Abo.Client.Core.Managers;
using Abo.Client.WP.Silverlight.ViewModels.OnlineHub.Blocks;

namespace Abo.Client.WP.Silverlight.ViewModels.OnlineHub
{
    public class AggregatedOnlineHubViewModel : BaseViewModel
    {
        public AggregatedOnlineHubViewModel(
            SearchPlayersViewModel search,
            BlacklistViewModel blackListViewModel,
            FriendsViewModel friendsViewModel,
            HallOfFameViewModel hallOfFameViewModel,
            OnlineProfileViewModel onlineProfileViewModel,
            PlayersOnlineHubViewModel playersOnlineHubViewModel,
            ChatOnlineHubViewModel chat)
        {
            Search = search;
            Blacklist = blackListViewModel;
            Friends = friendsViewModel;
            HallOfFame   = hallOfFameViewModel;
            Players = playersOnlineHubViewModel;
            Chat = chat;
            Profile = onlineProfileViewModel;
        }

        public OnlineProfileViewModel Profile { get; set; }
        public SearchPlayersViewModel Search { get; private set; }
        public BlacklistViewModel Blacklist { get; private set; }
        public FriendsViewModel Friends { get; private set; }
        public HallOfFameViewModel HallOfFame { get; private set; }
        public PlayersOnlineHubViewModel Players { get; private set; }
        public ChatOnlineHubViewModel Chat { get; private set; }
    }
}
