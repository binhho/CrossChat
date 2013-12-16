using System;
using Abo.Client.Core.Model.Messaging;

namespace Abo.Client.WP.Silverlight.ViewModels.Messages
{
    public class RemovedModershipNotificationViewModel : MessageViewModel
    {
        private readonly RemovedModershipNotificationEvent _rmEvent;

        public RemovedModershipNotificationViewModel(RemovedModershipNotificationEvent rmEvent)
        {
            _rmEvent = rmEvent;
        }
        public string ActorName { get { return _rmEvent.ActorName; } }

        public string TargetName { get { return _rmEvent.TargetName; } }

        public Guid TargetId { get { return _rmEvent.TargetId; } }
    }
}