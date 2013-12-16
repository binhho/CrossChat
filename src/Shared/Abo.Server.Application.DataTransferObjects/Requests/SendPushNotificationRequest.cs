namespace Abo.Server.Application.DataTransferObjects.Requests
{
    public class SendPushNotificationRequest : RequestBase
    {
        public int TargetId { get; set; }
    }
    public class SendPushNotificationResponse : ResponseBase
    {
        public bool IsReceived { get; set; }
    }
}
