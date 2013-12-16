using System;
using System.Globalization;
using System.Windows.Data;
using Abo.Client.Core.Model.Astral;
using Abo.Client.WP.Silverlight.Assets;
using Abo.Server.Application.DataTransferObjects.Messages;

namespace Abo.Client.WP.Silverlight.Converters
{
    public class ElementImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is ElementTypeEnum))
                return value;

            return string.Format(AssetsPathes.ElementImagePath, ((ElementTypeEnum) value).ToString().ToLowerInvariant());
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}