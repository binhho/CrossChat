using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Abo.Client.Core.Managers;
using Abo.Client.Core.Model;
using Abo.Client.Core.Model.Enums;
using Abo.Client.Wpf.Infrastructure;
using Abo.Client.Wpf.ViewModels.Messages;
using Abo.Utils.Collections;
using Autofac;
using GalaSoft.MvvmLight.Command;

namespace Abo.Client.Wpf.ViewModels
{
    public class MainViewModel : AboViewModelBase
    {
        private readonly AppManager _appManager = null;
        private readonly AccountManager _accountManager = null;
        private readonly ChatManager _chatManager = null;
        private readonly SearchManager _searchManager;
        private IRoObservableCollection<PlayerViewModel> _chatPlayers;
        private IRoObservableCollection<MessageViewModel> _messages;
        private string _subject;
        private string _input;
        private User[] _searchResult;
        private string _searchQuery;
        private bool _isSearching;

        public MainViewModel()
        {
            IsBusy = true;
            var container = Bootstrapper.Run();
            _appManager = container.Resolve<AppManager>();
            _chatManager = container.Resolve<ChatManager>();
            _accountManager = container.Resolve<AccountManager>();
            _searchManager = container.Resolve<SearchManager>();

            InitAsync();
        }

        private async void InitAsync()
        {
            var result = await _appManager.InitAsync();
            if (result != AuthenticationResult.Success)
            {
                string name = "WpfClient_" + new Random().Next(0, 99999);
                var regResult = await _accountManager.Register(name, "123", 13, true, "Belarus", -22);
                var authResult = await _appManager.InitAsync();
            }
            var reloadChatTask = _chatManager.ReloadChat();
            var reloadPlayersTask = _chatManager.ReloadUsers();
            
            ChatPlayers = _chatManager.OnlineUsers.Select(i => new PlayerViewModel(i), UIDispatcher.Current);
            Messages = _chatManager.History.Select(MessageFactory.Create, UIDispatcher.Current);

            Subject = _chatManager.Subject;
            _chatManager.SubjectChanged += (s, e) => UIDispatcher.Current.Dispatch(() => Subject = _chatManager.Subject);

            await Task.WhenAll(reloadChatTask, reloadPlayersTask);

            IsBusy = false;
        }

        public IRoObservableCollection<MessageViewModel> Messages
        {
            get { return _messages; }
            set { SetProperty(ref _messages, value); }
        }

        public IRoObservableCollection<PlayerViewModel> ChatPlayers
        {
            get { return _chatPlayers; }
            set { SetProperty(ref _chatPlayers, value); }
        }

        public User[] SearchResult
        {
            get { return _searchResult; }
            set { SetProperty(ref _searchResult, value); }
        }

        public string Subject
        {
            get { return _subject; }
            set { SetProperty(ref _subject, value); }
        }

        public string Input
        {
            get { return _input; }
            set { SetProperty(ref _input, value); }
        }

        public ICommand SendCommand
        {
            get
            {
                return new RelayCommand(() =>{
                    _chatManager.SendMessage(Input);
                    Input = string.Empty;
                }, () => !string.IsNullOrEmpty(Input));
            }
        }

        public string SearchQuery
        {
            get { return _searchQuery; }
            set { SetProperty(ref _searchQuery, value); }
        }

        public bool IsSearching
        {
            get { return _isSearching; }
            set { SetProperty(ref _isSearching, value); }
        }

        public ICommand SearchCommand
        {
            get
            {
                return new RelayCommand(async () =>
                {
                    IsSearching = true;
                    SearchResult = await _searchManager.SearchAsync(SearchQuery);
                    IsSearching = false;
                }, () => !string.IsNullOrEmpty(SearchQuery));
            }
        }
    }
}
