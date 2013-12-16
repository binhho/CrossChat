using System.Threading.Tasks;
using System.Windows.Input;
using Abo.Client.Core.Managers;
using Abo.Client.WP.Silverlight.Infrastructure;
using Abo.Client.WP.Silverlight.ViewModels.Messages;
using Abo.Utils.Collections;
using GalaSoft.MvvmLight.Command;

namespace Abo.Client.WP.Silverlight.ViewModels.OnlineHub.Blocks
{
    public class ChatOnlineHubViewModel : OnlineHubViewModelBase
    {
        private readonly ChatManager _chatManager;
        private IRoObservableCollection<MessageViewModel> _chatHistory;
        private string _inputText;
        private string _subject;

        public ChatOnlineHubViewModel(ChatManager chatManager)
        {
            _chatManager = chatManager;
        }

        protected override async Task OnReload()
        {
            await _chatManager.ReloadChat();
            ChatHistory = _chatManager.History.Select(MessageFactory.Create, UIDispatcher.Current);
            _chatManager.SubjectChanged += (s, e) => UIDispatcher.Current.Dispatch(() => Subject = _chatManager.Subject);
            Subject = _chatManager.Subject;
        }
        
        public string InputText
        {
            get { return _inputText; }
            set { SetProperty(ref _inputText, value); }
        }

        public string Subject
        {
            get { return _subject; }
            set { SetProperty(ref _subject, value); }
        }

        public ICommand SendTextCommand
        {
            get { return new RelayCommand(SendText); }
        }
        
        private void SendText()
        {
            if (string.IsNullOrEmpty(InputText))
                return;

            _chatManager.SendMessage(InputText);
            InputText = string.Empty;
        }

        public IRoObservableCollection<MessageViewModel> ChatHistory
        {
            get { return _chatHistory; }
            set { SetProperty(ref _chatHistory, value); }
        }
    }
}
