using System;
using System.Windows.Data;

namespace Abo.Client.WP.Silverlight.Seedwork.Converters
{
    public class ZeroToValueConverter : IValueConverter
    {
        public object ValueForZero { get; set; }
        public object ValueForNotZero { get; set; }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                return (int)value == 0 ? this.ValueForZero : this.ValueForNotZero;
            }
            catch
            {
                return (double)value == 0 ? this.ValueForZero : this.ValueForNotZero;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
