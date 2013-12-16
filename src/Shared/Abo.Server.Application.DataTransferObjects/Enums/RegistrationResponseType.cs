namespace Abo.Server.Application.DataTransferObjects.Enums
{
    public enum RegistrationResponseType
    {
        Success,
        NameIsInUse,
        InvalidData,
        OldClient,
        ServerIsBusy,
        Banned
    }
}