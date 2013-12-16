using System;
using Abo.Server.Application.DataTransferObjects.Messages;

namespace Abo.Server.Application.DataTransferObjects.Requests
{
    public class GetUserDetailsRequest : RequestBase
    {
        public int UserId { get; set; }
        public string Name { get; set; }
    }
    public class GetUserDetailsResponse : ResponseBase
    {
        public UserDto User { get; set; }
        public bool IsFriend { get; set; }
        public bool IsInBlacklist { get; set; }
    }
}