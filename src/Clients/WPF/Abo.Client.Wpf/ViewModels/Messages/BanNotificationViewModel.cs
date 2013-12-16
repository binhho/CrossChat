using Abo.Client.Core.Model.Messaging;

namespace Abo.Client.Wpf.ViewModels.Messages
{
    public class BanNotificationViewModel : MessageViewModel
    {
        private readonly BanNotificationEvent _banEvent;

        public BanNotificationViewModel(BanNotificationEvent banEvent)
        {
            _banEvent = banEvent;
        }

        public string TargetName { get { return _banEvent.TargetName; } }

        public string ActorName { get { return _banEvent.ActorName; } }

        public string Reason { get { return _banEvent.Reason; } }
    }
}