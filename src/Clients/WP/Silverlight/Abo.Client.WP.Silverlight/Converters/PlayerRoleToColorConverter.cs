using System;
using System.Globalization;
using System.Windows.Data;
using Abo.Client.Core.Model.Enums;

namespace Abo.Client.WP.Silverlight.Converters
{
    public class PlayerRoleToValueConverter : IValueConverter
    {
        public object Player { get; set; }
        public object Moderator { get; set; }
        public object Admin { get; set; } 

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is UserRole))
                return Player;
            var role = (UserRole) value;
            switch (role)
            {
                case UserRole.Player:
                    return Player;
                case UserRole.Moderator:
                    return Moderator;
                case UserRole.Admin:
                    return Admin;
                default:
                    return Player;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
