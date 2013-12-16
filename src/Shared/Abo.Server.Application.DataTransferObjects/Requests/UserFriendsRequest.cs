using Abo.Server.Application.DataTransferObjects.Messages;

namespace Abo.Server.Application.DataTransferObjects.Requests
{
    public class UserFriendsRequest : RequestBase
    {
    }
    public class UserFriendsResponse : ResponseBase
    {
        public UserDto[] Friends { get; set; }
    }
}