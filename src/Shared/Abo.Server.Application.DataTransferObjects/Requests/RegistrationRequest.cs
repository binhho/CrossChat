﻿using Abo.Server.Application.DataTransferObjects.Enums;
using Abo.Server.Application.DataTransferObjects.Messages;

namespace Abo.Server.Application.DataTransferObjects.Requests
{
    public class RegistrationRequest : RequestBase
    {
        public string Name { get; set; }
        public string Huid { get; set; }
        public string Password { get; set; }
        public string Country { get; set; }
        public string Comments { get; set; }
        public int Age { get; set; }
        public int PhotoId { get; set; }
        public bool Sex { get; set; }
        public string PushUri { get; set; }
    }
    public class RegistrationResponse : ResponseBase
    {
        public RegistrationResponseType Result { get; set; }
        public UserDto User { get; set; }
    }
}
