using System.Windows;
using System.Windows.Controls;
using Abo.Client.Wpf.ViewModels.Messages;

namespace Abo.Client.Wpf.Controls
{
    public class MessageItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate TextMessage { get; set; }

        public DataTemplate BanNotification { get; set; }

        public DataTemplate DevoiceNotification { get; set; }

        public DataTemplate GrantedModershipNotification { get; set; }

        public DataTemplate RemovedModershipNotification { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is TextMessageViewModel)
                return TextMessage;
            if (item is BanNotificationViewModel)
                return BanNotification;
            if (item is DevoiceNotificationViewModel)
                return DevoiceNotification;
            if (item is GrantedModershipNotificationViewModel)
                return GrantedModershipNotification;
            if (item is RemovedModershipNotificationViewModel)
                return RemovedModershipNotification;
            
            return base.SelectTemplate(item, container);
        }
    }
}
