using Abo.Server.Application.DataTransferObjects.Messages;

namespace Abo.Server.Application.DataTransferObjects.Requests
{
    public class GetOnlineUsersRequest : RequestBase { }
    public class GetOnlineUsersResponse : ResponseBase
    {
        public UserDto[] Users { get; set; }
    }
}