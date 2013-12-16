using System;
using Abo.Server.Domain.Seedwork;
using Abo.Utils;

namespace Abo.Server.Domain.Entities
{
    public class PublicMessage : Entity
    {
        public PublicMessage() { }

        public PublicMessage(User author, string body)
        {
            Author = author;
            AuthorName = author.Name;
            Timestamp = DateTime.UtcNow;
            Body = body.CutIfLonger();
            if (string.IsNullOrEmpty(Body))
                throw new ArgumentNullException();
            Timestamp = DateTime.UtcNow;
        }
        
        public virtual User Author { get; private set; }

        public DateTime Timestamp { get; private set; }

        public string AuthorName { get; private set; }

        public string Body { get; private set; }
    }
}
