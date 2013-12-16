using Abo.Client.Core.Model.Astral;
using Abo.Server.Application.DataTransferObjects.Messages;

namespace Abo.Client.WP.Silverlight.Views.DesignTimeViewModels
{
    public class DT_ElementInfoViewModel
    {
        public ElementTypeEnum ElementType
        {
            get { return ElementTypeEnum.Fire; }
        }

        public int Mana
        {
            get { return 3; }
        }

        public string OverlayText
        {
            get { return "+2"; }
        }
    }
}