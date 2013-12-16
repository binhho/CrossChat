using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abo.Client.WP.Silverlight.Views.DesignTimeViewModels
{
    public class DT_FieldViewModel
    {
        public dynamic Field
        {
            get
            {
                return new
                {
                    IsEmpty = false,
                    Card = new
                    {
                        Damage = 9,
                        Health = 36,
                        Cost = 10,
                    }

                };
            }
        }

        public string FieldName
        {
            get { return "5"; }
        }

        public string OverlayText
        {
            get { return "-10"; }
        }
    }
}
