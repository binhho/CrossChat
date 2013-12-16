using System;
using System.Globalization;
using System.Windows.Media;
using Abo.Utils;
using Cimbalino.Phone.Toolkit.Converters;

namespace Abo.Client.WP.Silverlight.Converters
{
    public class PlayerStatusToForegroundConverter : MultiValueConverterBase
    {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.IsNullOrEmpty())
                return null;

            if (values[0].ToBoolean())
                return new SolidColorBrush(Colors.Red);
            if (values[1].ToBoolean())
                return new SolidColorBrush(Colors.Green);
            return new SolidColorBrush(Colors.White);
        }

        public override object[] ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
