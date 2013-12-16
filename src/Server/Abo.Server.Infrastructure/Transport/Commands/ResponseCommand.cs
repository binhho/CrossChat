using Abo.Server.Infrastructure.Protocol;

namespace Abo.Server.Infrastructure.Transport.Commands
{
    public class ResponseCommand : AboCommandBase
    {
        public override void ExecuteAstralCommand(AboSession session, byte[] data)
        {
            session.AppendResponse(data);
        }

        protected override CommandNames CommandName
        {
            get { return CommandNames.Response; }
        }
    }
}
