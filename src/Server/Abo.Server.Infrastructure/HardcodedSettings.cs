using Abo.Server.Application.Contracts;

namespace Abo.Server.Infrastructure
{
    /// <summary>
    /// temporarily implimentation
    /// </summary>
    public class HardcodedSettings : ISettings
    {
        public bool IsServerBusy { get; private set; }
        public string ServerAddress { get { return "localhost"; } }
        public int ServerPort { get { return 3322; } }
        public string Subject { get { return "Welcome to Astral Battles Online 3 server!"; } }
        public int LastMessagesCount { get { return 20; } }
        public int ComposeRandomDuelsEachXSeconds { get { return 5; }}
        public string ImagesLocalFolder { get { return "D:\\Abo3_resource\\"; } }//@"C:\inetpub\wwwroot\abo3"; } }//
        public int ThumbnailSize { get { return 64; } }
        public int HallOfFameCount { get { return 25; } }
    }
}
