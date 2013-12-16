using System;
using Abo.Client.Core.Model.Messaging;

namespace Abo.Client.WP.Silverlight.ViewModels.Messages
{
    public static class MessageFactory
    {
        public static MessageViewModel Create(Event @event)
        {
            if (@event is TextMessage)
                return new TextMessageViewModel(@event as TextMessage);
            if (@event is DevoiceNotificationEvent)
                return new DevoiceNotificationViewModel(@event as DevoiceNotificationEvent);
            if (@event is BanNotificationEvent)
                return new BanNotificationViewModel(@event as BanNotificationEvent);
            if (@event is GrantedModershipNotificationEvent)
                return new GrantedModershipNotificationViewModel(@event as GrantedModershipNotificationEvent);
            if (@event is RemovedModershipNotificationEvent)
                return new RemovedModershipNotificationViewModel(@event as RemovedModershipNotificationEvent);

            throw new NotImplementedException();
        }
    }
}
