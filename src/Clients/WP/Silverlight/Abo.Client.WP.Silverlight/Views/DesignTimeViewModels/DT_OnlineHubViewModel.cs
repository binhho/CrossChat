using System;
using Abo.Client.Core.Model.Enums;
using Abo.Client.WP.Silverlight.ViewModels.Messages;
using Abo.Utils.Collections;

namespace Abo.Client.WP.Silverlight.Views.DesignTimeViewModels
{
    public class DT_OnlineHubViewModel
    {
        public DT_ChatOnlineHubViewModel Chat { get { return new DT_ChatOnlineHubViewModel(); } }

        public dynamic Friends
        {
            get
            {
                return new
                {
                    Friends = new []
                    {
                        new { Name = "Egorko", Country = "Belarus", IsModer=false, IsAdmin=false, PhotoId = GenerateRandomPhotoId() },
                        new { Name = "Michael Jakson", Country = "Norway", IsModer=true, IsAdmin=false, PhotoId = GenerateRandomPhotoId() },
                        new { Name = "Alexander_", Country = "France", IsModer=false, IsAdmin=false, PhotoId = GenerateRandomPhotoId() },
                        new { Name = "Ivan Ivanov ", Country = "Ukrain", IsModer=false, IsAdmin=true, PhotoId = GenerateRandomPhotoId() },
                    }
                };
            }
        }
        
        public dynamic Players
        {
            get
            {
                return new
                {
                    AvailablePlayers = new[]
                    {
                        new { Name = "Egorko", Country = "Belarus", Role = UserRole.Player, PhotoId = GenerateRandomPhotoId() },
                        new { Name = "Michael Jakson", Country = "Norway", Role = UserRole.Moderator, PhotoId = GenerateRandomPhotoId() },
                        new { Name = "Alexander_", Country = "France", Role = UserRole.Admin, PhotoId = GenerateRandomPhotoId() },
                        new { Name = "Ivan Ivanov ", Country = "Ukrain", Role = UserRole.Player, PhotoId = GenerateRandomPhotoId() },
                        new { Name = "Long name long name name", Country = "Ukrain", Role = UserRole.Player, PhotoId = GenerateRandomPhotoId() },
                    },
                    BusyPlayers = new[]
                    {
                        new { Name = "Egorko", Country = "Belarus", Role = UserRole.Player, PhotoId = GenerateRandomPhotoId() },
                        new { Name = "Michael Jakson", Country = "Norway", Role = UserRole.Moderator, PhotoId = GenerateRandomPhotoId() },
                        new { Name = "Alexander_", Country = "France", Role = UserRole.Admin, PhotoId = GenerateRandomPhotoId() },
                        new { Name = "Intervention[O_o]", Country = "Ukrain", Role = UserRole.Player, PhotoId = GenerateRandomPhotoId() },
                    },
                };
            }
        }

        public dynamic HallOfFame
        {
            get
            {
                return new
                {
                    Top = new[]
                    {
                        new { Name = "Egorko", Country = "Belarus", Role = UserRole.Player, Position = 1, PhotoId = GenerateRandomPhotoId() }, 
                        new { Name = "Steve Ballmer", Country = "Norway", Role = UserRole.Player, Position = 2, PhotoId = GenerateRandomPhotoId() }, 
                        new { Name = "George", Country = "France", Role = UserRole.Player, Position = 3, PhotoId = GenerateRandomPhotoId() }, 
                        new { Name = "John Doe", Country = "Austria", Role = UserRole.Moderator, Position = 4, PhotoId = GenerateRandomPhotoId() }, 
                        new { Name = "Bill Gates", Country = "German", Role = UserRole.Player, Position = 5, PhotoId = GenerateRandomPhotoId() }, 
                        new { Name = "Egorko", Country = "Belarus", Role = UserRole.Player, Position =6, PhotoId = GenerateRandomPhotoId() }, 
                        new { Name = "Steve Ballmer", Country = "Norway", Role = UserRole.Player, Position = 7, PhotoId = GenerateRandomPhotoId() }, 
                        new { Name = "George", Country = "France", Role = UserRole.Player, Position = 8, PhotoId = GenerateRandomPhotoId() }, 
                        new { Name = "John Doe", Country = "Austria", Role = UserRole.Moderator, Position =9, PhotoId = GenerateRandomPhotoId() }, 
                        new { Name = "Bill Gates", Country = "German", Role = UserRole.Player, Position = 10, PhotoId = GenerateRandomPhotoId() }, 
                        new { Name = "Egorko", Country = "Belarus", Role = UserRole.Player, Position = 11, PhotoId = GenerateRandomPhotoId() }, 
                        new { Name = "Steve Ballmer", Country = "Norway", Role = UserRole.Player, Position = 12, PhotoId = GenerateRandomPhotoId() }, 
                        new { Name = "George", Country = "France", Role = UserRole.Player, Position = 13, PhotoId = GenerateRandomPhotoId() }, 
                        new { Name = "John Doe", Country = "Austria", Role = UserRole.Moderator, Position = 14, PhotoId = GenerateRandomPhotoId() }, 
                        new { Name = "Bill Gates", Country = "German", Role = UserRole.Player, Position = 15, PhotoId = GenerateRandomPhotoId() }, 
                    },
                };
            }
        }

        private Random _random = new Random();

        private int GenerateRandomPhotoId()
        {
            return _random.Next(1, 70)*-1;
        }
    }

    public class DT_ChatOnlineHubViewModel
    {
        public string Subject {get { return "Test subject http://code.google.com/test reofeowjf wif ei fhwiue hfiuwe fiuwbeiuf niwe"; }}

        public ExtendedObservableCollection<DT_MessageViewModel> ChatHistory
        {
            get
            {
                return new ExtendedObservableCollection<DT_MessageViewModel>
                {
                    new DT_MessageViewModel { Body = "Hello!", AuthorName = "Egor", Timestamp = DateTime.Now},
                    new DT_MessageViewModel { Body = "War and Peace", AuthorName = "Tolstoy_Leo", Timestamp = DateTime.Now},
                };
            }
        }
    }

    public class DT_MessageViewModel : MessageViewModel
    {
        public string Body { get; set; }
        public string AuthorName { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
