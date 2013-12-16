using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abo.Client.WP.Silverlight.Views.DesignTimeViewModels
{
    public class DT_BattlefieldViewModel
    {
        public DT_BattlefieldPlayerViewModel Opponent
        {
            get
            {
                return new DT_BattlefieldPlayerViewModel();
            }
        }

        public DT_BattlefieldPlayerViewModel Me
        {
            get
            {
                return new DT_BattlefieldPlayerViewModel();
            }
        }
    }
}
