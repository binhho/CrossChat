using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abo.Server.Application.Seedwork;
using Abo.Server.Application.Sessions;
using Abo.Utils;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;

namespace Abo.Server.Infrastructure.Transport
{
    public class AboSocketServer : AppServer<AboSession, BinaryRequestInfo>, ISessionManager
    {
        private readonly TransportPerformanceMeasurer _performanceMeasurer = ServiceLocator.Resolve<TransportPerformanceMeasurer>();

        public AboSocketServer()
            : base(new DefaultReceiveFilterFactory<AboReceiveFilter, BinaryRequestInfo>())
        {
        }

        protected override void ExecuteCommand(AboSession session, BinaryRequestInfo requestInfo)
        {
            _performanceMeasurer.HandleIncomingBytes(requestInfo.Body.LongLength);
            base.ExecuteCommand(session, requestInfo);
        }

        public Dictionary<int, ISession> GetActiveSessions()
        {
            var sessions = base.GetSessions(s => s.IsAuthorized && s.IsOpen).ToDictionarySafe(i => i.User.Id, i => i as ISession);
            return sessions;
        }

        public void SendToEachChatSessions<T>(T data) where T : class
        {
            var allSessions = GetActiveSessions();
            Task.Run(() => Parallel.ForEach(allSessions, session => session.Value.Send(data)));
        }

        public void SendToEachChatSessionsExcept<T>(T data, int exceptionPlayerId) where T : class
        {
            var allSessions = GetActiveSessions();
            
            if (allSessions.ContainsKey(exceptionPlayerId))
                allSessions.Remove(exceptionPlayerId);

            Task.Run(() => Parallel.ForEach(allSessions, session => session.Value.Send(data)));
        }

        public ISession FindSessionByPlayerId(int playerId)
        {
            return base.GetSessions(s => s.IsAuthorized && s.User.Id == playerId && s.IsOpen).Last();
        }

        public void CloseSessionByPlayerId(int playerId)
        {
            var sessions = base.GetSessions(s => s.IsAuthorized && s.User.Id == playerId && s.IsOpen).ToList();
            foreach (var session in sessions)
            {
                try
                {
                    session.Close();
                }
                catch { }
            }
        }

        protected override void OnNewSessionConnected(AboSession session)
        {
            _performanceMeasurer.HandleNewConnection();
            session.Authorized += session_OnAuthorized;
            base.OnNewSessionConnected(session);
        }

        protected override void OnSessionClosed(AboSession session, CloseReason reason)
        {
            session.Authorized -= session_OnAuthorized;
            base.OnSessionClosed(session, reason);
            if (session.IsAuthorized)
            {
                AuthenticatedUserDisconnected(this, new SessionEventArgs(session));
            }
        }

        private void session_OnAuthorized(object sender, EventArgs e)
        {
            AuthenticatedUserConnected(this, new SessionEventArgs(sender as ISession));
        }

        public event EventHandler<SessionEventArgs> AuthenticatedUserConnected = delegate { };

        public event EventHandler<SessionEventArgs> AuthenticatedUserDisconnected = delegate { };
    }
}
