using Abo.Server.Application.DataTransferObjects.Messages;

namespace Abo.Server.Application.DataTransferObjects.Requests
{
    public class UserBlacklistRequest : RequestBase
    {
    }
    public class UserBlacklistResponse : ResponseBase
    {
        public UserDto[] Blacklist { get; set; }
    }
}