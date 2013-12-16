using System;
using Abo.Client.Core.Model.Messaging;

namespace Abo.Client.Wpf.ViewModels.Messages
{
    public class TextMessageViewModel : MessageViewModel
    {
        private readonly TextMessage _textMessage;

        public TextMessageViewModel(TextMessage textMessage)
        {
            _textMessage = textMessage;
        }

        public string Body
        {
            get { return _textMessage.Body; }
        }

        public string AuthorName
        {
            get { return _textMessage.AuthorName; }
        }

        public DateTime Timestamp
        {
            get { return _textMessage.Timestamp.ToLocalTime(); }
        }

        public bool IsAdmin
        {
            get { return _textMessage.IsAdmin; }
        }

        public bool IsModerator
        {
            get { return _textMessage.IsModerator; }
        }
    }
}
