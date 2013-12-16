using Abo.Client.Core.Model.Messaging;

namespace Abo.Client.Wpf.ViewModels.Messages
{
    public class DevoiceNotificationViewModel : MessageViewModel
    {
        private readonly DevoiceNotificationEvent _devoiceEvent;

        public DevoiceNotificationViewModel(DevoiceNotificationEvent devoiceEvent)
        {
            _devoiceEvent = devoiceEvent;
        }

        public string TargetName { get { return _devoiceEvent.TargetName; } }

        public string ActorName { get { return _devoiceEvent.ActorName; } }

        public string Reason { get { return _devoiceEvent.Reason; } }
    }
}