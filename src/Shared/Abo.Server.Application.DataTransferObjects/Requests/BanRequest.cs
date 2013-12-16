﻿using System;
using Abo.Server.Application.DataTransferObjects.Enums;

namespace Abo.Server.Application.DataTransferObjects.Requests
{
    public class BanRequest : RequestBase
    {
        /// <summary>
        /// The reason why target has been banned\devoiced
        /// </summary>
        public string Reason { get; set; }

        /// <summary>
        /// Target user Id
        /// </summary>
        public int TargetUserId { get; set; }

        public bool Ban { get; set; }
    }
    public class BanResponse : ResponseBase
    {
        public BanResponseType Result { get; set; }
    }
}