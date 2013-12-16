using System;
using System.Windows.Input;
using Abo.Client.Core.Model;
using Abo.Client.Core.Model.Enums;
using GalaSoft.MvvmLight.Command;

namespace Abo.Client.Wpf.ViewModels
{
    public class PlayerViewModel : AboViewModelBase
    {
        private readonly User _user;

        public PlayerViewModel(User user)
        {
            _user = user;
            user.PropertyChanged += player_OnPropertyChanged;
        }

        private void player_OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            RaisePropertyChangedToUIThread(e.PropertyName);
        }

        public Guid Id
        {
            get { return _user.Id; }
        }

        public bool IsInChat
        {
            get { return _user.IsInChat; }
        }

        public string Name
        {
            get { return _user.Name; }
        }

        public bool Sex
        {
            get { return _user.Sex; }
        }

        public int Age
        {
            get { return _user.Age; }
        }

        public int GamesCount
        {
            get { return _user.GamesCount; }
        }

        public int Defeats
        {
            get { return _user.Defeats; }
        }

        public int Level
        {
            get { return _user.Level; }
        }

        public int VictoriesCount
        {
            get { return _user.VictoriesCount; }
        }

        public int Xp
        {
            get { return _user.Xp; }
        }

        public bool IsDevoiced
        {
            get { return _user.IsDevoiced; }
        }

        public string Country
        {
            get { return _user.Country; }
        }

        public int PhotoId
        {
            get { return _user.PhotoId; }
        }

        public UserRole Role
        {
            get { return _user.Role; }
        }
    }

    public class HallOfFamePlayer : PlayerViewModel
    {
        public HallOfFamePlayer(int position, User user)
            : base(user)
        {
            Position = position;
        }

        public int Position { get; set; }
    }
}
