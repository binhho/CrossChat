using System;
using System.Windows.Data;

namespace Abo.Client.WP.Silverlight.Seedwork.Converters
{
    public class NullToValueConverter : IValueConverter
    {
        public object ValueForNull { get; set; }
        public object ValueForNotNull { get; set; }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value == null ? this.ValueForNull : this.ValueForNotNull;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
