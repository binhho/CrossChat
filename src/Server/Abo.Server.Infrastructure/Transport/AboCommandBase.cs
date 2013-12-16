using System;
using Abo.Server.Application.Seedwork;
using Abo.Server.Domain.Entities;
using Abo.Server.Infrastructure.Protocol;
using Abo.Server.Infrastructure.Serialization;
using Abo.Utils.Logging;
using SuperSocket.SocketBase.Command;
using SuperSocket.SocketBase.Protocol;

namespace Abo.Server.Infrastructure.Transport
{
    public abstract class AboCommandBase : CommandBase<AboSession, BinaryRequestInfo>
    {
        protected static readonly ILogger Logger = LogFactory.GetLogger<AboCommandBase>();

        protected static readonly IDtoSerializer DtoSerializer = ServiceLocator.Resolve<IDtoSerializer>();

        //protected static readonly CommandParser CommandParser = ServiceLocator.Resolve<CommandParser>();

        /// <summary>
        /// Command is allowed to executed by anonymouses
        /// </summary>
        protected virtual bool AllowAnonymousAccess { get { return false; } }

        /// <summary>
        /// True means the command available even if server is in updating state
        /// </summary>
        protected virtual bool AlwaysAvailable { get { return false; } }

        /// <summary>
        /// Command requires admin rights to be executed
        /// </summary>
        protected virtual bool RequiresAdminAccess { get { return false; } }

        /// <summary>
        /// Command handler
        /// </summary>
        public abstract void ExecuteAstralCommand(AboSession session, byte[] data);

        /// <summary>
        /// Command name
        /// </summary>
        protected abstract CommandNames CommandName { get; }

        public override string Name
        {
            get { return CommandName.ToString(); }
        }

        public sealed override void ExecuteCommand(AboSession session, BinaryRequestInfo requestInfo)
        {
            if (RequiresAdminAccess)
            {
                if (!session.IsAuthorized || session.User.Role != UserRole.Admin)
                {
                    //Logger.Warn("Sending access denided (ADMIN command) to {0}", Name);
                    //session.SendCommand(Name, Answers.AccessDenided);
                    return;
                }
            }
            else if (!AllowAnonymousAccess && !session.IsAuthorized)
            {
                //Logger.Warn("Sending access denided to {0}", Name);
                //session.SendCommand(Name, Answers.AccessDenided);
                return;
            }

            try
            {
                ExecuteAstralCommand(session, requestInfo.Body);
            }
            catch (Exception exc)
            {
                //Logger.ErrorException("Command '" + Commands.GetCommandFriendlyName(Name) + "' error!", exc);
            }
        }
    }
}
