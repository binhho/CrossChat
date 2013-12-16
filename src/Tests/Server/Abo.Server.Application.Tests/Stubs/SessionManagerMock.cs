using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Abo.Server.Application.Sessions;
using Abo.Server.Domain.Entities;
using Moq;

namespace Abo.Server.Application.Tests.Stubs
{
    internal class SessionManagerMock : ISessionManager
    {
        private Random _rand = new Random();
        private readonly List<ISession> _sessions = new List<ISession>();

        public Dictionary<int, ISession> GetActiveSessions()
        {
            return _sessions.ToDictionary(i => i.User.Id, i => i);
        }

        public void SendToEachChatSessions<T>(T data) where T : class
        {
            _sessions.ForEach(s => s.Send(data));
        }

        public void SendToEachChatSessionsExcept<T>(T data, int exceptionPlayerId) where T : class
        {
            _sessions.Where(i => i.User.Id != exceptionPlayerId).ToList().ForEach(s => s.Send(data));
        }

        public ISession FindSessionByPlayerId(int playerId)
        {
            return _sessions.Find(i => i.User.Id == playerId);
        }

        public void CloseSessionByPlayerId(int id)
        {
        }

        public Mock<ISession> AppendPlayerAsSession(string name, PlayerState playerState = PlayerState.InChat)
        {
            var sessionMock = new Mock<ISession>();
            var player = new User(name, "111", Guid.NewGuid().ToString(), true, 20, "", "Belarus");
            sessionMock.SetupGet(i => i.User).Returns(player);
            _sessions.Add(sessionMock.Object);
            return sessionMock;
        }

        public event EventHandler<SessionEventArgs> AuthenticatedUserConnected = delegate { };

        public event EventHandler<SessionEventArgs> AuthenticatedUserDisconnected = delegate { };
    }
}
