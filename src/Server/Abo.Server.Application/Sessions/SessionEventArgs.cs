using System;

namespace Abo.Server.Application.Sessions
{
    public class SessionEventArgs : EventArgs
    {
        public ISession Session { get; private set; }

        public SessionEventArgs(ISession session)
        {
            Session = session;
        }
    }
}
