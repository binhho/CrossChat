using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Abo.Client.WP.Silverlight.Seedwork.Notifications;

namespace Abo.Client.WP.Silverlight.Notifications.Toast
{
    public class ToastNotificator
    {
        public void DisplayJustText(string message)
        {
            var textblock = new TextBlock();
            textblock.Text = message;
            textblock.VerticalAlignment = VerticalAlignment.Center;
            textblock.Margin = new Thickness(12, 0, 0, 0);
            textblock.TextTrimming = TextTrimming.None;
            textblock.TextWrapping = TextWrapping.Wrap;
            textblock.Foreground = new SolidColorBrush(Colors.White);
            textblock.FontSize = 20;

            ToastPromtsHostControl.EnqueueItem(textblock, null, Color.FromArgb(255, 21, 96, 144));
        }
    }
}
