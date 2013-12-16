using System.Collections.Generic;
using System.Linq;
using Abo.Server.Application.DataTransferObjects;
using Abo.Server.Application.DataTransferObjects.Messages;
using Abo.Server.Application.Sessions;
using Abo.Utils;

namespace Abo.Server.Application.Services.Helpers
{
    public class ProfileChangesNotificator
    {
        private readonly ISessionManager _sessionManager;

        public ProfileChangesNotificator(ISessionManager sessionManager)
        {
            _sessionManager = sessionManager;
        }

        public void NotifyEverybodyInChatAboutProfileChanges(int playerId, Dictionary<string, object> properties)
        {
            if (properties.IsNullOrEmpty())
                return;

            var info = new UserPropertiesChangedInfo();
            info.Properties = properties.Select(i => new PropertyValuePair(i.Key, i.Value)).ToArray();
            info.UserId = playerId;
            _sessionManager.SendToEachChatSessions(info);
        }
    }
}
