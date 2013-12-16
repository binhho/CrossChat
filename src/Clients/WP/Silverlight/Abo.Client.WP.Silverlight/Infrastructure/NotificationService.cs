using System.Windows;
using Abo.Client.Core.Infrastructure.Contracts.View;

namespace Abo.Client.WP.Silverlight.Infrastructure
{
    public class NotificationService : INotificationService
    {
        public void ShowYouAreDevoicedMessage()
        {
            MessageBox.Show("You have been devoiced");
        }
    }
}
