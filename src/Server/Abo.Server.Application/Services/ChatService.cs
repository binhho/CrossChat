using System.Collections.Generic;
using System.Linq;
using Abo.Server.Application.Contracts;
using Abo.Server.Application.DataTransferObjects.Enums;
using Abo.Server.Application.DataTransferObjects.Messages;
using Abo.Server.Application.DataTransferObjects.Requests;
using Abo.Server.Application.Seedwork;
using Abo.Server.Application.Services.Helpers;
using Abo.Server.Application.Sessions;
using Abo.Server.Domain.Entities;
using Abo.Server.Domain.Exceptions;
using Abo.Server.Domain.Seedwork;

namespace Abo.Server.Application.Services
{
    public class ChatService : AppService
    {
        private readonly ISessionManager _sessionManager;
        private readonly ISettings _settings;
        private readonly ProfileChangesNotificator _profileChangesNotificator;
        private readonly IPushNotificator _pushNotificator;

        public ChatService(
            ISessionManager sessionManager,
            ISettings settings,
            ProfileChangesNotificator profileChangesNotificator,
            ITransactionFactory transactionFactory,
            IUnitOfWorkFactory unitOfWorkFactory,
            IPushNotificator pushNotificator)
            : base(unitOfWorkFactory, transactionFactory)
        {
            _sessionManager = sessionManager;
            _settings = settings;
            _profileChangesNotificator = profileChangesNotificator;
            _pushNotificator = pushNotificator;
            _sessionManager.AuthenticatedUserConnected += SessionManagerOnAuthenticatedUserConnected;
            _sessionManager.AuthenticatedUserDisconnected += SessionManagerOnAuthenticatedUserDisconnected;
        }

        /// <summary>
        /// NOTE: we don't need an app-level ack for this method :-)
        /// </summary>
        public void PublicMessage(ISession session, PublicMessageRequest msgRequest)
        {
            if (session.User.IsDevoiced)
            {
                session.Send(new YouAreDevoicedNotification());
                return;
            }

            PublicMessage message = null;
            using (var uow = UnitOfWorkFactory.Create())
            {
                uow.Attach(session.User);
                message = new PublicMessage(session.User, msgRequest.Body);
                uow.PublicMessageRepository.Add(message);
                uow.Commit();
            }

            var dto = message.ProjectedAs<PublicMessageDto>();
            dto.Role = (UserRoleEnum)(int)session.User.Role;
            _sessionManager.SendToEachChatSessions(dto);
        }

        public GrantModershipResponse GrantModership(ISession session, GrantModershipRequest request)
        {
            var response = request.CreateResponse<GrantModershipResponse>();
            if (session.User.Role != UserRole.Admin)
            {
                //access denied
                return response;
            }

            User targetUser = null;
            using (var uow = UnitOfWorkFactory.Create())
            {
                targetUser = uow.UsersRepository.FirstMatching(UserSpecification.Id(request.TargetUserId));
                if (targetUser == null)
                {
                    //User not found
                    return response;
                }

                targetUser.RemoveModeratorship();
                uow.Commit();
            }

            response.Success = true;
            _sessionManager.SendToEachChatSessions(new ModershipGrantedInfo
                {
                    ActorName = session.User.Name,
                    TargetName = targetUser.Name,
                    TargetId = targetUser.Id,
                });

            //success
            return response;
        }

        public RemoveModershipResponse RemoveModership(ISession session, RemoveModershipRequest request)
        {
            var response = request.CreateResponse<RemoveModershipResponse>();
            if (session.User.Role != UserRole.Admin)
            {
                //access denied
                return response;
            }

            User targetUser = null;
            using (var uow = UnitOfWorkFactory.Create())
            {
                targetUser = uow.UsersRepository.FirstMatching(UserSpecification.Id(request.TargetUserId));
                if (targetUser == null)
                {
                    //User not found
                    return response;
                }

                targetUser.RemoveModeratorship();
                uow.Commit();
            }

            response.Success = true;
            _sessionManager.SendToEachChatSessions(new ModershipRemovedInfo
                {
                    ActorName = session.User.Name,
                    TargetName = targetUser.Name,
                    TargetId = targetUser.Id,
                });

            //success
            return response;
        }

        public DevoiceResponse Devoice(ISession session, DevoiceRequest request)
        {
            var response = request.CreateResponse<DevoiceResponse>();

            User targetUser = null;

            using (var uow = UnitOfWorkFactory.Create())
            {
                targetUser = uow.UsersRepository.FirstMatching(UserSpecification.Id(request.TargetUserId));
                if (targetUser != null)
                {
                    try
                    {
                        if (request.Devoice)
                        {
                            targetUser.Devoice(session.User);
                        }
                        else
                        {
                            targetUser.BringVoiceBack(session.User);
                        }
                        response.Result = DevoiceResponseType.Success;
                        uow.Commit();
                    }
                    catch (ModeratorsRightsRequiredException)
                    {
                        response.Result = DevoiceResponseType.Failed;
                    }
                }
            }
            
            if (response.Result == DevoiceResponseType.Success && targetUser != null)
            {
                //send a notification to everybody that we've devoiced him\her
                _sessionManager.SendToEachChatSessions(
                    new DevoiceNotification
                    {
                        ActorName = session.User.Name, 
                        Reason = request.Reason,
                        Devoice = request.Devoice,
                        TargetId = session.User.Id,
                        TargetName = targetUser.Name
                    });
            }
            return response;
        }

        public BanResponse Ban(ISession session, BanRequest request)
        {
            var response = request.CreateResponse<BanResponse>();

            User targetUser = null;

            using (var uow = UnitOfWorkFactory.Create())
            {
                targetUser = uow.UsersRepository.FirstMatching(UserSpecification.Id(request.TargetUserId));
                if (targetUser != null)
                {
                    try
                    {
                        if (request.Ban)
                            targetUser.Ban(session.User);
                        else
                            targetUser.UnBan(session.User);

                        response.Result = BanResponseType.Success;
                        uow.Commit();
                    }
                    catch (ModeratorsRightsRequiredException)
                    {
                        response.Result = BanResponseType.Failed;
                    }
                }
            }

            if (response.Result == BanResponseType.Success && targetUser != null)
            {
                //let's kick him\her from the server!
                _sessionManager.CloseSessionByPlayerId(targetUser.Id);

                //send a notification to everybody that we've banned him\her
                _sessionManager.SendToEachChatSessions(
                    new BanNotification
                    {
                        Ban = request.Ban,
                        ActorName = session.User.Name,
                        Reason = request.Reason,
                        TargetId = session.User.Id,
                        TargetName = targetUser.Name
                    });
            }

            return response;
        }

        /// <summary>
        /// Resets user's photo to default one
        /// </summary>
        public ResetPhotoResponse ResetPhoto(ISession session, ResetPhotoRequest request)
        {
            var response = request.CreateResponse<ResetPhotoResponse>();
            try
            {
                using (var uow = UnitOfWorkFactory.Create())
                {
                    var target = uow.UsersRepository.FirstMatching(UserSpecification.Id(request.TargetId));
                    if (target != null)
                    {
                        response.NewPhotoId = target.ResetPhoto(session.User);
                        uow.Commit();
                        response.Success = true;
                    }
                }
            }
            catch (ModeratorsRightsRequiredException)
            {
                response.Success = false;
            }
            if (response.Success)
            {
                _profileChangesNotificator.NotifyEverybodyInChatAboutProfileChanges(request.TargetId, new Dictionary<string,object> {{ "PhotoId", response.NewPhotoId }});
            }
            return response;
        }

        public LastMessageResponse GetLastMessages(ISession session, LastMessageRequest request)
        {
            var response = request.CreateResponse<LastMessageResponse>(); 

            List<PublicMessageDto> messages = null;
            using (var uow = UnitOfWorkFactory.Create())
            {
                messages = uow.PublicMessageRepository
                    .TakeLast(_settings.LastMessagesCount)
                    .ProjectedAsCollection<PublicMessageDto>()
                    .ToList();
            }
            response.Subject = _settings.Subject;
            response.Messages = messages.ToArray();
            return response;
        }

        public GetOnlineUsersResponse GetOnlineUsers(ISession session, GetOnlineUsersRequest request)
        {
            var response = request.CreateResponse<GetOnlineUsersResponse>(); 

            var players = new List<UserDto>(); 
            foreach (var onlineSession in _sessionManager.GetActiveSessions())
            {
                var playerDto = onlineSession.Value.User.ProjectedAs<UserDto>();
                players.Add(playerDto);
            }

            response.Users = players.ToArray();
            return response;
        }

        public SendPushNotificationResponse SendPushNotification(ISession session, SendPushNotificationRequest request)
        {
            var response = request.CreateResponse<SendPushNotificationResponse>();

            using (var uow = UnitOfWorkFactory.Create())
            {
                var user = uow.UsersRepository.FirstMatching(UserSpecification.Id(request.TargetId));
                if (user != null && !string.IsNullOrWhiteSpace(user.PushUri))
                {
                    response.IsReceived = _pushNotificator.Send(user.PushUri, string.Format("{0} invites you to online", session.User.Name));
                }
            }

            return response;
        }

        private void SessionManagerOnAuthenticatedUserDisconnected(object sender, SessionEventArgs e)
        {
            //send notification to everyone that someone has left
            _sessionManager.SendToEachChatSessionsExcept(
                new LeftUserInfo { UserId = e.Session.User.Id }, e.Session.User.Id);
        }

        private void SessionManagerOnAuthenticatedUserConnected(object sender, SessionEventArgs e)
        {
            //send notification to everyone that someone has joined
            _sessionManager.SendToEachChatSessionsExcept(
                new JoinedUserInfo { User = e.Session.User.ProjectedAs<UserDto>() }, e.Session.User.Id);
        }
    }
}
