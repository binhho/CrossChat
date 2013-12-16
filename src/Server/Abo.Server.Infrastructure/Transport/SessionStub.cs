using System.Text;
using System.Threading.Tasks;
using Abo.Server.Application.Sessions;
using Abo.Server.Domain.Entities;

namespace Abo.Server.Infrastructure.Transport
{
    public class SessionStub : ISession
    {
        public void SetPlayer(Player player)
        {
            Player = player;
        }

        public Player Player { get; private set; }

        public bool IsRegistered { get { return true; } }

        public bool IsOpen { get { return true; } }

        public void Send<T>(T obj)
        {
        }

        public SessionState SessionState { get { return SessionState.InChat; } }
    }
}
