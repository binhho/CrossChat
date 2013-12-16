using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Abo.Client.Core.Managers;
using Abo.Client.Core.Model;
using Abo.Client.WP.Silverlight.Notifications.Toast;
using GalaSoft.MvvmLight.Command;

namespace Abo.Client.WP.Silverlight.ViewModels
{
    public class OnlinePlayerDetailsViewModel : BaseViewModel
    {
        private readonly SearchManager _searchManager;
        private readonly InputTextViewModel _inputTextViewModel;
        private readonly ChatManager _chatManager;
        private readonly ToastNotificator _toastNotificator;
        private readonly FriendsManager _friendsManager;
        private readonly BlacklistManager _blacklistManager;
        private bool _isLoading;
        private User _user;
        private bool _isFriend;
        private bool _isInBlacklist;
        private int _personalVictories;
        private int _personalDefeats;

        public OnlinePlayerDetailsViewModel(
            SearchManager searchManager, 
            InputTextViewModel inputTextViewModel,
            ChatManager chatManager, 
            ToastNotificator toastNotificator,
            FriendsManager friendsManager, 
            BlacklistManager blacklistManager)
        {
            _searchManager = searchManager;
            _inputTextViewModel = inputTextViewModel;
            _chatManager = chatManager;
            _toastNotificator = toastNotificator;
            _friendsManager = friendsManager;
            _blacklistManager = blacklistManager;
        }

        public async void Show(int playerId)
        {
            IsFriend = false;
            IsInBlacklist = false;
            User = null;
            IsLoading = true;

            Show();
            var details = await _searchManager.GetDetails(playerId);
            ApplyPlayer(details.User);

            IsFriend = details.IsMyFriend;
            IsInBlacklist = details.IsInMyBlacklist;
        }

        private void ApplyPlayer(User user)
        {
            if (user == null)
            {
                RequestClose();
                MessageBox.Show("Oops, User wasn't found ;(");
            }
            else
            {
                User = user;
            }
            IsLoading = false;
        }

        public User User
        {
            get { return _user; }
            set { SetProperty(ref _user, value); }
        }

        public bool IsLoading
        {
            get { return _isLoading; }
            set { SetProperty(ref _isLoading, value); }
        }

        public bool IsFriend
        {
            get { return _isFriend; }
            set { SetProperty(ref _isFriend, value); }
        }

        public bool IsInBlacklist
        {
            get { return _isInBlacklist; }
            set { SetProperty(ref _isInBlacklist, value); }
        }

        public int PersonalVictories
        {
            get { return _personalVictories; }
            set { SetProperty(ref _personalVictories, value); }
        }

        public int PersonalDefeats
        {
            get { return _personalDefeats; }
            set { SetProperty(ref _personalDefeats, value); }
        }

        public ICommand BanCommand
        {
            get { return new RelayCommand(Ban); }
        }

        private async void Ban()
        {
            IsLoading = true;
            if (User.IsBanned)
            {
                if (await _chatManager.UnBan(User.Id))
                {
                    User.IsBanned = false;
                }
            }
            else
            {
                var reason = await _inputTextViewModel.Ask("Please, enter the reason.");
                if (reason == null)
                {
                    IsLoading = false;
                    return;
                }
                if (await _chatManager.Ban(User.Id, reason))
                {
                    User.IsBanned = true;
                }
                else
                {
                    MessageBox.Show("Failed attempt to ban the user");
                }
            }
            IsLoading = false;
            RequestClose();
        }


        public ICommand DevoiceCommand
        {
            get { return new RelayCommand(Devoice); }
        }

        private async void Devoice()
        {
            IsLoading = true;
            if (User.IsDevoiced)
            {
                if (await _chatManager.BringVoiceBack(User.Id))
                {
                    User.IsDevoiced = false;
                }
            }
            else
            {
                var reason = await _inputTextViewModel.Ask("Please, enter the reason.");
                if (reason == null)
                {
                    IsLoading = false;
                    return;
                }
                if (await _chatManager.Devoice(User.Id, reason))
                {
                    User.IsDevoiced = true;
                }
                else
                {
                    MessageBox.Show("Failed attempt to devoice the user");
                }
            }
            IsLoading = false;
            RequestClose();
        }

        public ICommand ResetPhotoCommand
        {
            get { return new RelayCommand(ResetPhoto); }
        }

        private async void ResetPhoto()
        {
            IsLoading = true;
            await _chatManager.ResetPhoto(User.Id);
            IsLoading = false;
            RequestClose();
        }

        public ICommand SendPushNotificationCommand
        {
            get { return new RelayCommand(SendPushNotification); }
        }

        private async void SendPushNotification()
        {
            IsLoading = true;
            string targetName = User.Name; 
            var task = _chatManager.SendPushInvitation(User.Id);
            await Task.Delay(2000);
            IsLoading = false;
            bool received = await task;
            _toastNotificator.DisplayJustText(received
                ? string.Format("{0} has received your push notification.", targetName)
                : string.Format("{0} is unavailable", targetName));
        }


        public ICommand AddToFriendsCommand
        {
            get { return new RelayCommand(AddToFriends); }
        }

        private async void AddToFriends()
        {
            IsLoading = true;
            await _friendsManager.AddToFriendsAsync(User);
            IsFriend = true;
            IsLoading = false;
            RequestClose();
        }


        public ICommand RemoveFromFriendsCommand
        {
            get { return new RelayCommand(RemoveFromFriends); }
        }

        private async void RemoveFromFriends()
        {
            IsLoading = true;
            await _friendsManager.RemoveFromFriendsAsync(User);
            IsFriend = false;
            IsLoading = false;
            RequestClose();
        }


        public ICommand AddToBlacklistCommand
        {
            get { return new RelayCommand(AddToBlacklist); }
        }

        private async void AddToBlacklist()
        {
            IsLoading = true;
            await _blacklistManager.AddToBlacklistAsync(User);
            IsInBlacklist = true;
            IsLoading = false;
            RequestClose();
        }


        public ICommand RemoveFromBlacklistCommand
        {
            get { return new RelayCommand(RemoveFromBlacklist); }
        }

        private async void RemoveFromBlacklist()
        {
            IsLoading = true;
            await _blacklistManager.RemoveFromBlacklistAsync(User);
            IsInBlacklist = false;
            IsLoading = false;
            RequestClose();
        }
    }
}
