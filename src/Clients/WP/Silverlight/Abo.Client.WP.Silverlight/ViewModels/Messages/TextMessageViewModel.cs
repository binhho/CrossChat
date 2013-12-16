using System;
using Abo.Client.Core.Model.Messaging;

namespace Abo.Client.WP.Silverlight.ViewModels.Messages
{
    public class TextMessageViewModel : MessageViewModel
    {
        private readonly TextMessage _textMessage;

        public TextMessageViewModel(TextMessage textMessage)
        {
            _textMessage = textMessage;
            Timestamp = _textMessage.Timestamp.ToLocalTime();
        }

        public string Body
        {
            get { return _textMessage.Body; }
        }

        public string AuthorName
        {
            get { return _textMessage.AuthorName; }
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
