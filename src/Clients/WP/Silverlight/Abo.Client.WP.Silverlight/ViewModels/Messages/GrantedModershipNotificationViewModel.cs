using System;
using Abo.Client.Core.Model.Messaging;

namespace Abo.Client.WP.Silverlight.ViewModels.Messages
{
    public class GrantedModershipNotificationViewModel : MessageViewModel
    {
        private readonly GrantedModershipNotificationEvent _gmEvent;

        public GrantedModershipNotificationViewModel(GrantedModershipNotificationEvent gmEvent)
        {
            _gmEvent = gmEvent;
        }

        public string ActorName { get { return _gmEvent.ActorName; } }

        public string TargetName { get { return _gmEvent.TargetName; } }

        public Guid TargetId { get { return _gmEvent.TargetId; } }
    }
}