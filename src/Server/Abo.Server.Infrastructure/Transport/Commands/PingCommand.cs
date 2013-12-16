using System;
using Abo.Server.Infrastructure.Protocol;

namespace Abo.Server.Infrastructure.Transport.Commands
{
    public class PingCommand : AboCommandBase
    {
        protected override bool AllowAnonymousAccess { get { return true; } }

        protected override bool AlwaysAvailable { get { return true; } }

        public override void ExecuteAstralCommand(AboSession session, byte[] data)
        {
        }

        protected override CommandNames CommandName
        {
            get { return CommandNames.Ping; }
        }
    }
}
