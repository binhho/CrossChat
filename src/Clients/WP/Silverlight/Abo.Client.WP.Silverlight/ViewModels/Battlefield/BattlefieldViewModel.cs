using Abo.Client.Core.Model;

namespace Abo.Client.WP.Silverlight.ViewModels.Battlefield
{
    public class BattlefieldViewModel : BaseViewModel
    {
        public BattlefieldPlayerViewModel Opponent { get; set; }

        public BattlefieldPlayerViewModel Me { get; set; }

        public void Initialize(User opponent, User me)
        {
            
        }
    }
}
