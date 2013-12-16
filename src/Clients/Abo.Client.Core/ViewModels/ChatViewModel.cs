using Abo.Client.Core.Infrastructure.Proxies;
using Cirrious.MvvmCross.ViewModels;

namespace Abo.Client.Core.ViewModels
{
    public class ChatViewModel : MvxViewModel
    {
        private readonly ChatServiceProxy _chatServiceProxy;

        public ChatViewModel(ChatServiceProxy chatServiceProxy)
        {
            _chatServiceProxy = chatServiceProxy;
        }
    }
}
