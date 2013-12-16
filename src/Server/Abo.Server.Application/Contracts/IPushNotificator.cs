namespace Abo.Server.Application.Contracts
{
    public interface IPushNotificator
    {
        bool Send(string pushUri, string text);
    }
}
