using System;
using System.Globalization;
using System.Windows.Data;
using Abo.Client.Core.Model.Astral.Cards;

namespace Abo.Client.WP.Silverlight.Converters
{
    public class IsCreatureToValue : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            CreatureCard creature = value as CreatureCard;
            return creature == null ? ValueForFalse : ValueForTrue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        public object ValueForTrue { get; set; }

        public object ValueForFalse { get; set; }
    }
}
