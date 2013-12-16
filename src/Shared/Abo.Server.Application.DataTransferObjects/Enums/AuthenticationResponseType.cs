namespace Abo.Server.Application.DataTransferObjects.Enums
{
    public enum AuthenticationResponseType
    {
        Success,
        ReconnectFailed,
        InvalidNameOrPassword,
        Banned,
        ServerIsBusy,
        OldClient,
    }
}