using System;
using System.Windows.Data;

namespace Abo.Client.WP.Silverlight.Seedwork.Converters
{
    public class StringLengthToValueConverter : IValueConverter
    {
        public object ValueForEmpty { get; set; }
        public object ValueForFilled { get; set; }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var str = (String)value;
            if (String.IsNullOrEmpty(str))
            {
                return this.ValueForEmpty;
            }
            else
            {
                return this.ValueForFilled;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
