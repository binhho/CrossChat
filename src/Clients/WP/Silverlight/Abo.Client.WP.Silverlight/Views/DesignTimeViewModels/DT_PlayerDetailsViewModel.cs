using Abo.Client.Core.Model;
using Abo.Client.Core.Model.Enums;

namespace Abo.Client.WP.Silverlight.Views.DesignTimeViewModels
{
    public class DT_PlayerDetailsViewModel
    {
        public User User
        {
            get
            {
                return new User
                {
                    Age = 23,
                    Country = "Belarus",
                    GamesCount = 10,
                    Role = UserRole.Moderator,
                    IsDevoiced = true,
                    IsBanned = true,
                    IsInChat = true,
                    Name = "Egorko_DT"
                };
            }
        }
    }
}
