using System;

namespace Abo.Client.WP.Silverlight.ViewModels.Messages
{
    public partial class MessageViewModel : BaseViewModel
    {
        public DateTime Timestamp { get; protected set; }

        public override string ToString()
        {
            return string.Empty;
        }
    }
}

