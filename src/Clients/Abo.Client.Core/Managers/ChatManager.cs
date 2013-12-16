using System;
using System.Linq;
using System.Threading.Tasks;
using Abo.Client.Core.Infrastructure.Contracts.View;
using Abo.Client.Core.Infrastructure.Proxies;
using Abo.Client.Core.Model;
using Abo.Client.Core.Model.Messaging;
using Abo.Server.Application.DataTransferObjects.Enums;
using Abo.Server.Application.DataTransferObjects.Messages;
using Abo.Server.Application.DataTransferObjects.Requests;
using Abo.Utils.Collections;

namespace Abo.Client.Core.Managers
{
    public class ChatManager : ManagerBase
    {
        private readonly AccountManager _accountManager;
        private readonly ExtendedCcObservableCollection<Event> _messagesSubject = null;
        private readonly ExtendedCcObservableCollection<User> _onlinePlayersSubject = null;
        private readonly ChatServiceProxy _chatServiceProxy = null;
        private readonly INotificationService _notificationService;
        private string _subject;

        public ChatManager(ConnectionManager connectionManager, 
            ChatServiceProxy chatServiceProxy,
            INotificationService notificationService,
            AccountManager accountManager) : base(connectionManager)
        {
            _chatServiceProxy = chatServiceProxy;
            _notificationService = notificationService;
            _accountManager = accountManager;
            
            History = _messagesSubject = new ExtendedCcObservableCollection<Event>();
            OnlineUsers = _onlinePlayersSubject = new ExtendedCcObservableCollection<User>();
        }

        /// <summary>
        /// Reloads only messages and subject
        /// </summary>
        public async Task ReloadChat()
        {
            var chatStatus = await _chatServiceProxy.GetLastMessages(new LastMessageRequest());
            Subject = chatStatus.Subject;
            _messagesSubject.Clear();
            if (chatStatus.Messages != null)
                _messagesSubject.AddRange(chatStatus.Messages.Select(ToEntity<TextMessage>).Reverse());
        }

        /// <summary>
        /// Reloads only online players
        /// </summary>
        public async Task ReloadUsers()
        {
            var chatStatus = await _chatServiceProxy.GetOnlineUsers(new GetOnlineUsersRequest());
            _onlinePlayersSubject.Clear();
            if (chatStatus.Users != null)
                _onlinePlayersSubject.AddRange(chatStatus.Users.Select(ToEntity<User>));
        }

        /// <summary>
        /// Kick & ban (only for admins)
        /// </summary>
        public async Task<bool> Ban(int userId, string reason)
        {
            var result = await _chatServiceProxy.Ban(new BanRequest { Reason = reason, TargetUserId = userId, Ban = true });
            return result.Result == BanResponseType.Success;
        }

        /// <summary>
        /// Kick & ban (only for admins)
        /// </summary>
        public async Task<bool> UnBan(int userId)
        {
            var result = await _chatServiceProxy.Ban(new BanRequest { TargetUserId = userId });
            return result.Result == BanResponseType.Success;
        }

        /// <summary>
        /// Shut somebody up (only for moders)
        /// </summary>
        public async Task<bool> Devoice(int userId, string reason)
        {
            var result = await _chatServiceProxy.Devoice(new DevoiceRequest { Reason = reason, TargetUserId = userId, Devoice = true });
            return result.Result == DevoiceResponseType.Success;
        }

        /// <summary>
        /// Shut somebody up (only for moders)
        /// </summary>
        public async Task<bool> BringVoiceBack(int userId)
        {
            var result = await _chatServiceProxy.Devoice(new DevoiceRequest { TargetUserId = userId });
            return result.Result == DevoiceResponseType.Success;
        }

        /// <summary>
        /// Reset photo (only for moders)
        /// </summary>
        public async Task<bool> ResetPhoto(int userId)
        {
            var result = await _chatServiceProxy.ResetPhoto(new ResetPhotoRequest {TargetId = userId});
            return result.Success;
        }

        /// <summary>
        /// Send a public message
        /// </summary>
        public void SendMessage(string message)
        {
            //_messagesSubject.Add(new Message(message));
            _chatServiceProxy.PublicMessage(new PublicMessageRequest { Body = message });
        }

        /// <summary>
        /// Fires on subject change
        /// </summary>
        public event EventHandler SubjectChanged = delegate { };

        /// <summary>
        /// Chat topic (subject)
        /// </summary>
        public string Subject
        {
            get { return _subject; }
            private set
            {
                _subject = value;
                SubjectChanged(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Chat messages
        /// </summary>
        public ICcRoObservableCollection<Event> History { get; private set; }

        /// <summary>
        /// Active players (online)
        /// </summary>
        public ICcRoObservableCollection<User> OnlineUsers { get; private set; }


        protected override void OnUnknownDtoReceived(BaseDto dto)
        {
            //TODO: REPLACE WITH REACTIVE EXTENSIONS!

            //messages:
            ToEntityAndAddToList<BanNotificationEvent, Event, BanNotification>(dto, _messagesSubject);
            ToEntityAndAddToList<TextMessage, Event, PublicMessageDto>(dto, _messagesSubject);
            ToEntityAndAddToList<DevoiceNotificationEvent, Event, DevoiceNotification>(dto, _messagesSubject);
            ToEntityAndAddToList<GrantedModershipNotificationEvent, Event, ModershipGrantedInfo>(dto, _messagesSubject);
            ToEntityAndAddToList<RemovedModershipNotificationEvent, Event, ModershipRemovedInfo>(dto, _messagesSubject);

            //users
            ToEntityAndAddToList<User, UserDto>(dto, _onlinePlayersSubject);
            ToEntityAndAddToList<User, JoinedUserInfo>(dto, _onlinePlayersSubject, d => d.User);

            //special case (remove User):
            var leftUserInfo = dto as LeftUserInfo;
            if (leftUserInfo != null)
                RemoveEntityFromList(_onlinePlayersSubject, i => i.Id == leftUserInfo.UserId);

            var playerProfileChanges = dto as UserPropertiesChangedInfo;
            if (playerProfileChanges != null)
            {
                UpdatePropertiesForList(_onlinePlayersSubject, p => p.Id == playerProfileChanges.UserId, playerProfileChanges.Properties);
            }

            //update property IsDevoiced for players
            var devoiceNotification = dto as DevoiceNotification;
            if (devoiceNotification != null)
            {
                using (OnlineUsers.EnterSafetyRead())
                {
                    var user = OnlineUsers.FirstOrDefault(i => i.Id == devoiceNotification.TargetId);
                    if (user != null)
                        user.IsDevoiced = devoiceNotification.Devoice;
                }
            }

            var youAreDevoicedNotification = dto as YouAreDevoicedNotification;
            if (youAreDevoicedNotification != null)
            {
                _notificationService.ShowYouAreDevoicedMessage();
            }
        }

        public async Task<bool> SendPushInvitation(int userId)
        {
            var response = await _chatServiceProxy.SendPushNotification(new SendPushNotificationRequest {TargetId = userId});
            return response.IsReceived;
        }
    }
}
