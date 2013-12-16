using System;
using System.Globalization;
using System.Windows.Data;

namespace Abo.Client.WP.Silverlight.Converters
{
    public class IdToPhotoUrlConverter : IValueConverter
    {
        public const string FaceImagePath = "/Abo.Client.WP.Silverlight;component/Assets/Avatars/{0}.JPG";
        public const int LastFaceIndex = 79;

        public static string CustomImageUriTemplate = "http://" + GlobalConfig.IpAddress + "/abo3/{1}/{0}.jpg";

        public bool IsLarge { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is int) || ((int)value) == 0)
            {
                return string.Format(FaceImagePath, "face0");
            }
            int id = (int) value;
            if (id < 0)
            {
                id *= -1;
                if (id > 0 && id <= LastFaceIndex)
                    return string.Format(FaceImagePath, "face" + id);
                return string.Format(FaceImagePath, "face0");
            }
            else
            {
                return string.Format(CustomImageUriTemplate, id, IsLarge ? "l" : "s");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
