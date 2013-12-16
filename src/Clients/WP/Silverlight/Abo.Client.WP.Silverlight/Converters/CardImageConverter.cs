using System;
using System.Globalization;
using System.Windows.Data;
using Abo.Client.Core.Model.Astral.Cards;
using Abo.Client.WP.Silverlight.Assets;

namespace Abo.Client.WP.Silverlight.Converters
{
    public class CardImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Convert(value as Card);
        }

        public string Convert(Card card)
        {
            if (card == null)
                return null;
            string dir = AssetsPathes.CardImagePath + card.ElementType.ToString() + "/";
            //if (card is FakeCard && !((FakeCard)card).IsLocked)
            //    return "/Resources/Common/notsetfavoritecard.jpg";
            if (card is SpellCard)
                return dir + "Spells/" + card.Name + ".png";
            return dir + card.Name + ".jpg";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
