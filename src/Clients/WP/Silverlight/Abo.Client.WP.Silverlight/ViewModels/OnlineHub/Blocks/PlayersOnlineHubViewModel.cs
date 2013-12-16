using System.Threading.Tasks;
using System.Windows.Input;
using Abo.Client.Core.Managers;
using Abo.Client.Core.Model;
using Abo.Client.WP.Silverlight.Infrastructure;
using Abo.Utils.Collections;
using GalaSoft.MvvmLight.Command;

namespace Abo.Client.WP.Silverlight.ViewModels.OnlineHub.Blocks
{
    public class PlayersOnlineHubViewModel : OnlineHubViewModelBase
    {
        private readonly AppManager _appManager;
        private readonly FriendsManager _friendsManager;
        private readonly DuelsManager _duelsManager;
        private readonly BlacklistManager _blacklistManager;
        private readonly ChatManager _chatManager;
        private IRoObservableCollection<PlayerViewModel> _availablePlayers = new ExtendedCcObservableCollection<PlayerViewModel>();
        private IRoObservableCollection<PlayerViewModel> _busyPlayers = new ExtendedCcObservableCollection<PlayerViewModel>();
        private bool _ignoreDuelRequests;
        private bool _isSearchingRandomDuel;

        public PlayersOnlineHubViewModel(AppManager appManager,
            FriendsManager friendsManager,
            DuelsManager duelsManager,
            BlacklistManager blacklistManager,
            ChatManager chatManager)
        {
            _appManager = appManager;
            _friendsManager = friendsManager;
            _duelsManager = duelsManager;
            _blacklistManager = blacklistManager;
            _chatManager = chatManager;
        }

        protected override async Task OnReload()
        {
            await _chatManager.ReloadUsers();
            AvailablePlayers = _chatManager.OnlineUsers.Where(new AvailablePlayersCriterion(true)).Select(CreatePlayer, UIDispatcher.Current);
            BusyPlayers = _chatManager.OnlineUsers.Where(new AvailablePlayersCriterion(false)).Select(CreatePlayer, UIDispatcher.Current);
        }

        private PlayerViewModel CreatePlayer(User arg)
        {
            var vm = new PlayerViewModel(arg);
            return vm;
        }

        public IRoObservableCollection<PlayerViewModel> AvailablePlayers
        {
            get { return _availablePlayers; }
            set { SetProperty(ref _availablePlayers, value); }
        }

        public IRoObservableCollection<PlayerViewModel> BusyPlayers
        {
            get { return _busyPlayers; }
            set { SetProperty(ref _busyPlayers, value); }
        }

        public bool IgnoreDuelRequests
        {
            get { return _ignoreDuelRequests; }
            set { SetProperty(ref _ignoreDuelRequests, value); }
        }

        public bool IsSearchingRandomDuel
        {
            get { return _isSearchingRandomDuel; }
            set { SetProperty(ref _isSearchingRandomDuel, value); }
        }

        public ICommand RandomDuelCommand
        {
            get { return new RelayCommand(RandomDuel); }
        }

        private void RandomDuel()
        {
            if (IsSearchingRandomDuel)
            {
                IsSearchingRandomDuel = !IsSearchingRandomDuel;
                _duelsManager.RandomDuelSearch(true);
            }
            else
            {
                IsSearchingRandomDuel = !IsSearchingRandomDuel;
                _duelsManager.RandomDuelSearch(false);
            }
        }
    }



    public class AvailablePlayersCriterion : ICriterionProvider<User>
    {
        private readonly bool _available;

        public AvailablePlayersCriterion(bool available)
        {
            _available = available;
        }

        private class Criterion : Criterion<User>
        {
            public Criterion(User value, bool available) : base(value)
            {
                IsSuitable = (available && value.IsInChat) ^ (!available && !value.IsInChat);
                value.PropertyChanged += (s, e) =>
                {
                    if (e.PropertyName == "IsInChat")
                        IsSuitable = (available && value.IsInChat) ^ (!available && !value.IsInChat);
                };
            }
        }

        public Criterion<User> GetCriterion(User value)
        {
            return new Criterion(value, _available);
        }
    }
}
