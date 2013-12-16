using System;
using System.Globalization;
using System.Windows.Data;
using Abo.Client.WP.Silverlight.Assets.Flags;

namespace Abo.Client.WP.Silverlight.Converters
{
    public class CountryNameToFlagUriConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.Format(CountriesList.FileImagePathTemplate, value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
